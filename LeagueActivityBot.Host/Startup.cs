using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Hangfire;
using HangfireBasicAuthenticationFilter;
using LeagueActivityBot.BackgroundJobs;
using LeagueActivityBot.Controllers;
using LeagueActivityBot.Controllers.Configuration;
using LeagueActivityBot.Database;
using LeagueActivityBot.Host.Filters;
using LeagueActivityBot.Host.Infrastructure;
using LeagueActivityBot.Telegram;
using LeagueActivityBot.Riot;
using LeagueActivityBot.Riot.Configuration;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LeagueActivityBot.Host
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            var dbConnString = Configuration["App:DbConnectionString"];
            services.AddPostgreSqlStorage(options =>
            {
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.UseNpgsql(dbConnString);
            });
            
            services.AddSingleton(new MapperConfiguration(mc =>
            {
                mc.AddProfile(new RiotMappingProfile());
                mc.AddProfile(new ControllersMappingProfile());
            }).CreateMapper());
            
            services.AddMemoryCache();
            services.AddHealthChecks();
            services.AddRiot<RiotClientOptions>(options => Configuration.GetSection("App:Riot").Bind(options));
            services.AddBot();
            
            services.AddBackgroundJobs(dbConnString);
            services.AddNotifications<TelegramOptions>(options =>
            {
                options.TelegramBotApiKey = Configuration["App:Telegram:ApiKey"];
                options.TelegramChatId = long.Parse(Configuration["App:Telegram:ChatId"]);
                options.TelegramLogChatId = long.Parse(Configuration["App:Telegram:LogChatId"]);
            });

            services.AddMediatR(
                Assembly.GetAssembly(typeof(Telegram.Entry)), 
                Assembly.GetAssembly(typeof(Entry)),
                Assembly.GetAssembly(typeof(BackgroundJobs.Entry)));

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin() 
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });
            
            services.AddControllers(options =>
                {
                    options.Filters.Add<ExceptionFilter>();
                })
                .AddApi()
                .AddControllersAsServices()
                .ConfigureApiBehaviorOptions(o => o.InvalidModelStateResponseFactory = context =>
                {
                    return new BadRequestObjectResult(new
                    {
                        message = string.Join(" | ", context.ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage))
                    });
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger, IServiceProvider serviceProvider)
        {
            MigrationsRunner.ApplyMigrations(logger, serviceProvider, "LeagueActivityBot.Host").Wait();
            
            if (bool.Parse(Configuration["App:Startup:EnableSummonersSync"]))
            {
                SummonersInitializer.Initialize(serviceProvider).Wait();
            }
            
            if (bool.Parse(Configuration["App:Startup:EnableStartupNotification"]))
            {
                TelegramNotification.SendNotification(serviceProvider,"Service started").Wait();
            }

            app.UseCors("AllowAll");
            app.UseRouting();
            app.UseHealthChecks("/health");
            app.UseStaticFiles();
            app.UseMiddleware<ApiKeyMiddleware>();
            
            if (bool.Parse(Configuration["App:Hangfire:EnableDashboard"]))
            {
                app.UseHangfireDashboard("/hangfire", new DashboardOptions
                {
                    Authorization = new []
                    {
                        new HangfireCustomBasicAuthenticationFilter
                        {
                            User = "admin",
                            Pass = Configuration["App:Hangfire:DashboardPassword"]
                        }
                    }
                });
            }

            app.AddBackgroundJobs();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

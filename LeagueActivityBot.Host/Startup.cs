using System;
using System.Reflection;
using AutoMapper;
using LeagueActivityBot.BackgroundJobs;
using LeagueActivityBot.Calendar;
using LeagueActivityBot.Calendar.Integration;
using LeagueActivityBot.Controllers;
using LeagueActivityBot.Database;
using LeagueActivityBot.Host.Options;
using LeagueActivityBot.Telegram;
using LeagueActivityBot.Riot;
using LeagueActivityBot.Riot.Configuration;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

            }).CreateMapper());
            
            services.AddMemoryCache();
            services.AddHealthChecks();
            services.AddRiot<RiotClientOptions>(options => Configuration.GetSection("App:Riot").Bind(options));
            services.AddCalendar<CalendarClientOptions>(options => Configuration.GetSection("App:Calendar").Bind(options));

            services.AddBot<BotOptions>(options =>
            {
                options.SummonerNames = Configuration["App:SummonerNames"].Split(";");
            });
            
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
                Assembly.GetAssembly(typeof(BackgroundJobs.Entry)),
                Assembly.GetAssembly(typeof(Calendar.Entry)));

            services.AddMvc()
                .AddApi()
                .AddControllersAsServices();
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

            app.UseRouting();
            app.UseHealthChecks("/health");
            app.UseStaticFiles();

            if (bool.Parse(Configuration["App:Hangfire:EnableDashboard"]))
            {
                app.UseBackgroundJobsDashboard();
            }

            app.AddBackgroundJobs();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

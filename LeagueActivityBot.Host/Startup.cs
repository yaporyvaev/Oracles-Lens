using System;
using System.Reflection;
using AutoMapper;
using LeagueActivityBot.BackgroundJobs;
using LeagueActivityBot.Controllers;
using LeagueActivityBot.Database;
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
            services.AddPostgreSqlStorage(options =>
            {
                options.UseNpgsql(Configuration["App:DbConnectionString"]);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            
            services.AddSingleton(new MapperConfiguration(mc =>
            {
                mc.AddProfile(new RiotMappingProfile());

            }).CreateMapper());
            
            services.AddMemoryCache();
            services.AddHealthChecks();
            services.AddRiot<RiotClientOptions>(options => Configuration.GetSection("App:Riot").Bind(options));

            services.AddBot<BotOptions>(options =>
            {
                options.SummonerNames = Configuration["App:SummonerNames"].Split(";");
            });

            services.AddBackgroundJobs();
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

            services.AddMvc()
                .AddApi()
                .AddControllersAsServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger, IServiceProvider serviceProvider)
        {
            MigrationsRunner.ApplyMigrations(logger, serviceProvider, "LeagueActivityBot.Host").Wait();
            SummonersInitializer.Initialize(serviceProvider).Wait();
            TelegramNotification.SendOnStartedUpNotification(serviceProvider).Wait();
            
            app.UseRouting();
            app.UseHealthChecks("/health");
            app.UseStaticFiles();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

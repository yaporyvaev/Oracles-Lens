using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace LeagueActivityBot.BackgroundJobs
{
    public static class Entry
    {
        public static IServiceCollection AddBackgroundJobs(this IServiceCollection services, string dbConnection)
        {
            services.AddHangfire(x => x.UsePostgreSqlStorage(dbConnection));
            services.AddHangfireServer();

            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                var startGameCheckerJobKey = new JobKey("StartGameCheckerJob");
                q.AddJob<StartGameCheckerJob>(opts => opts.WithIdentity(startGameCheckerJobKey));
                q.AddTrigger(opts => opts
                    .ForJob(startGameCheckerJobKey)
                    .WithIdentity("StartGameCheckerJob-trigger")
                    .WithCronSchedule("0/40 * * ? * *"));

                var endGameCheckerJobKey = new JobKey("EndGameCheckerJob");
                q.AddJob<EndGameCheckerJob>(opts => opts.WithIdentity(endGameCheckerJobKey));
                q.AddTrigger(opts => opts
                    .ForJob(endGameCheckerJobKey)
                    .WithIdentity("endGameCheckerJob-trigger")
                    .WithCronSchedule("1 * * ? * *"));

                var clashScheduleCheckerJobKey = new JobKey("ClashScheduleCheckerJob");
                q.AddJob<ClashScheduleCheckerJob>(opts => opts.WithIdentity(clashScheduleCheckerJobKey));
                q.AddTrigger(opts => opts
                    .ForJob(clashScheduleCheckerJobKey)
                    .WithIdentity("clashScheduleCheckerJob-trigger")
                    .WithDailyTimeIntervalSchedule(a => a.StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(1, 0))));
                    //.WithDailyTimeIntervalSchedule(a => a.WithInterval(1, IntervalUnit.Minute))); //Debug schedule
            });
            
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            return services;
        }
        
        public static void UseBackgroundJobsDashboard(this IApplicationBuilder app)
        {
            app.UseHangfireDashboard();
        }
        
        public static void AddBackgroundJobs(this IApplicationBuilder app)
        {
            RecurringJob.AddOrUpdate<MatchSyncJob>(
                "MatchSyncJob",
                x => x.Sync(),
                Cron.Daily(4));
        }
    }
}
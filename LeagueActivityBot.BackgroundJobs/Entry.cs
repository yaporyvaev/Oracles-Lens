using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace LeagueActivityBot.BackgroundJobs
{
    public static class Entry
    {
        public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
        {
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
                    .WithCronSchedule("5 20 * ? * *"));
            });
            
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            
            return services;
        }
    }
}
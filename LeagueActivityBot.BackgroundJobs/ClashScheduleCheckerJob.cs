using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using JetBrains.Annotations;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Notifications.OnClashScheduleReceived;
using LeagueActivityBot.Services;
using MediatR;
using Quartz;

namespace LeagueActivityBot.BackgroundJobs
{
    [DisallowConcurrentExecution]
    [UsedImplicitly]
    public class ClashScheduleCheckerJob : IJob
    {
        private readonly IRiotClient _riotClient;
        
        public ClashScheduleCheckerJob(IRiotClient riotClient)
        {
            _riotClient = riotClient;
        }
        
        public async Task Execute(IJobExecutionContext context)
        {
            var clashInfos = await _riotClient.GetClashSchedule();
            
            if (clashInfos != null && clashInfos.Any())
            {
                var clashesToday = ClashService.GetClashesForADay(clashInfos, DateTime.Today).ToArray();
                if (clashesToday.Any())
                {
                    BackgroundJob.Schedule<IMediator>(m => m.Publish(new ClashAnnouncementNotification(clashesToday), CancellationToken.None), 
                        new DateTimeOffset(DateTime.SpecifyKind(DateTime.Today.AddHours(13), DateTimeKind.Local)));
                }
            }
        }
    }
}
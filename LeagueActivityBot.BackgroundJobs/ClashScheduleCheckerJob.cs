using System.Threading.Tasks;
using JetBrains.Annotations;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Notifications.OnClashScheduleReceived;
using MediatR;
using Quartz;

namespace LeagueActivityBot.BackgroundJobs
{
    [DisallowConcurrentExecution]
    [UsedImplicitly]
    public class ClashScheduleCheckerJob : IJob
    {
        private readonly IRiotClient _riotClient;
        private readonly IMediator _mediator;
        
        public ClashScheduleCheckerJob(IRiotClient riotClient, IMediator mediator)
        {
            _riotClient = riotClient;
            _mediator = mediator;
        }
        
        public async Task Execute(IJobExecutionContext context)
        {
            var clashInfos = await _riotClient.GetClashSchedule();
            
            if (clashInfos != null)
            {
                await _mediator.Publish(new OnClashScheduleReceivedNotification(clashInfos));
            }
        }
    }
}
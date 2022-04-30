using System.Threading.Tasks;
using JetBrains.Annotations;
using LeagueActivityBot.Services;
using Quartz;

namespace LeagueActivityBot.BackgroundJobs
{
    [DisallowConcurrentExecution]
    [UsedImplicitly]
    public class GameActivityCheckerJob : IJob
    {
        private readonly GameActivityChecker _activityChecker;

        public GameActivityCheckerJob(GameActivityChecker activityChecker)
        {
            _activityChecker = activityChecker;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _activityChecker.Check();
        }
    }
}
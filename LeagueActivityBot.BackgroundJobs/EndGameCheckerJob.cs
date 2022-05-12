using System.Threading.Tasks;
using JetBrains.Annotations;
using LeagueActivityBot.Services;
using Quartz;

namespace LeagueActivityBot.BackgroundJobs
{
    [DisallowConcurrentExecution]
    [UsedImplicitly]
    public class EndGameCheckerJob : IJob
    {
        private readonly EndGameChecker _endGameChecker;

        public EndGameCheckerJob(EndGameChecker endGameChecker)
        {
            _endGameChecker = endGameChecker;
        }
        
        public async Task Execute(IJobExecutionContext context)
        {
            await _endGameChecker.Check();
        }
    }
}
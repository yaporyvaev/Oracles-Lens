using System.Threading.Tasks;
using JetBrains.Annotations;
using LeagueActivityBot.Services;
using Quartz;

namespace LeagueActivityBot.BackgroundJobs.Jobs
{
    [DisallowConcurrentExecution]
    [UsedImplicitly]
    public class StartGameCheckerJob : IJob
    {
        private readonly StartGameChecker _startGameChecker;

        public StartGameCheckerJob(StartGameChecker startGameChecker)
        {
            _startGameChecker = startGameChecker;
        }
        
        public async Task Execute(IJobExecutionContext context)
        {
            await _startGameChecker.Check();
        }
    }
}
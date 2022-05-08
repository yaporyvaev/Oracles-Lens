using System.Threading.Tasks;
using LeagueActivityBot.Telegram.BotCommands.Abstractions;
using LeagueActivityBot.Telegram.BotCommands.GeneralStates;

namespace LeagueActivityBot.Telegram.BotCommands.CreatePool
{
    public class CreateBinaryAnswerPool : BaseCommand
    {
        public override Task<CommandState> Handle(long commandOwnerId, string payload)
        {
            return Task.FromResult(new CommandState(BotCommandsTypes.CreateBinaryAnswerPool, commandOwnerId, new FinishCommandHandlingState(payload)));
        }
    }
}
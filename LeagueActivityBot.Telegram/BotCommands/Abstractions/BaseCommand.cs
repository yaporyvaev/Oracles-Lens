using System.Threading.Tasks;

namespace LeagueActivityBot.Telegram.BotCommands.Abstractions
{
    public abstract class BaseCommand
    {
        public abstract Task<CommandState> Handle(long commandOwnerId, string payload);
    }
}
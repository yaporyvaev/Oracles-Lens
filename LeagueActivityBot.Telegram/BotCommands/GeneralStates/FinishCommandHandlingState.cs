using LeagueActivityBot.Telegram.BotCommands.Abstractions;

namespace LeagueActivityBot.Telegram.BotCommands.GeneralStates
{
    public class FinishCommandHandlingState : BaseState
    {
        public sealed override string Message { get; protected set; }
        
        public FinishCommandHandlingState(string message)
        {
            Message = message;
        }
    }
}
using LeagueActivityBot.Telegram.BotCommands.Abstractions;

namespace LeagueActivityBot.Telegram.BotCommands.GeneralStates
{
    public class ActionConfirmRequiredState : BaseState
    {
        public sealed override string Message { get; protected set; }

        public ActionConfirmRequiredState(string message)
        {
            Message = message;
        }
    }
}
namespace LeagueActivityBot.Telegram.BotCommands.Abstractions
{
    public abstract class BaseState
    {
        public abstract string Message { get; protected set; }
    }
}
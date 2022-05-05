using LeagueActivityBot.Telegram.BotCommands.Abstractions;

namespace LeagueActivityBot.Telegram.BotCommands.GeneralStates
{
    public class SummonerNameRequiredState : BaseState
    {
        public override string Message
        {
            get => "Enter summoner's name";
            protected set => throw new System.NotImplementedException();
        }
    }
}
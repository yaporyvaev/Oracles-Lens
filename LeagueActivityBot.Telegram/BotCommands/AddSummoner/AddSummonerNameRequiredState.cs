using LeagueActivityBot.Telegram.BotCommands.Abstractions;
using LeagueActivityBot.Telegram.BotCommands.GeneralStates;

namespace LeagueActivityBot.Telegram.BotCommands.AddSummoner
{
    public class AddSummonerNameInNotificationsRequiredState : BaseState
    {
        public override string Message
        {
            get => "Enter the name you want to see in notifications";
            protected set => throw new System.NotImplementedException();
        }
    }
}
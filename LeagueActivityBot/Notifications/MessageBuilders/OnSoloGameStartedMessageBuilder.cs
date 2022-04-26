using System.Text;

namespace LeagueActivityBot.Notifications.MessageBuilders
{
    public class OnSoloGameStartedMessageBuilder
    {
        public string Build(OnSoloGameStartedNotification notification)
        {
            var sb = new StringBuilder($"{notification.SummonerName} крыса, играет в соло! {GetPersonal(notification.SummonerName)}");
            return sb.ToString();
        } 
        
        private string GetPersonal(string summonerName)
        {
            if (summonerName == "Frostmeen") return "Стас, ты чё ебанулся?";

            return string.Empty;
        }
    }
}
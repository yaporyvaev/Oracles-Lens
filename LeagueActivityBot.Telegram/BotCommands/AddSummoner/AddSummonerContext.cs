using LeagueActivityBot.Entities;
using LeagueActivityBot.Telegram.BotCommands.Abstractions;

namespace LeagueActivityBot.Telegram.BotCommands.AddSummoner
{
    public class AddSummonerContext : BaseStateContext
    {
        public Summoner Summoner { get; set; }
    }
}
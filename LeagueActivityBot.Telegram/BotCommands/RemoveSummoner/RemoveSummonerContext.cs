using LeagueActivityBot.Entities;
using LeagueActivityBot.Telegram.BotCommands.Abstractions;

namespace LeagueActivityBot.Telegram.BotCommands.RemoveSummoner
{
    public class RemoveSummonerContext : BaseStateContext
    {
        public Summoner Summoner { get; set; }
    }
}
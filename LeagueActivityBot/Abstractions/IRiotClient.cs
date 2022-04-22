using System.Threading.Tasks;
using LeagueActivityBot.Models;

namespace LeagueActivityBot.Abstractions
{
    public interface IRiotClient
    {
        Task<SummonerInfo> GetSummonerInfoByName(string summonerName);
        Task<CurrentGameInfo> GetCurrentGameInfo(string summonerId);
        Task<MatchInfo> GetMatchInfo(long gameId);
    }
}
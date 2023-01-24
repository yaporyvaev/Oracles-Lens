using System.Collections.Generic;
using System.Threading.Tasks;
using LeagueActivityBot.Models;

namespace LeagueActivityBot.Abstractions
{
    public interface IRiotClient
    {
        Task<SummonerInfo> GetSummonerInfoByName(string summonerName);
        Task<SpectatorGameInfo> GetCurrentGameInfo(string summonerId);
        Task<MatchInfo> GetMatchInfo(string gameId);
        Task<MatchInfo> GetMatchInfo(long gameId);
        Task<List<string>> GetMatchIds(string summonerPuuid, int skip, int take);
        Task<LeagueInfo[]> GetLeagueInfo(string summonerId);
        Task<ClashInfo[]> GetClashSchedule();
    }
}
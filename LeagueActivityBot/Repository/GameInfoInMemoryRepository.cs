using System.Collections.Concurrent;
using LeagueActivityBot.Models;

namespace LeagueActivityBot.Repository
{
    public class GameInfoInMemoryRepository
    {
        //Remove magic numbers
        private ConcurrentDictionary<string, CurrentGameInfo> GameInfos { get; } = new ConcurrentDictionary<string, CurrentGameInfo>(2,8);
        private ConcurrentDictionary<string, long> LastGameIdMap { get; } = new ConcurrentDictionary<string, long>(2,8);

        public bool GameExists(string summonerName)
        {
            return GameInfos.ContainsKey(summonerName) && GameInfos[summonerName] != null;
        }
        
        public CurrentGameInfo GetGame(string summonerName)
        {
            return GameInfos[summonerName];
        }
        
        public void AddGame(string summonerName, CurrentGameInfo gameInfo)
        {
            GameInfos[summonerName] = gameInfo;
        }
        
        public void RemoveGame(string summonerName)
        {
            if (GameExists(summonerName))
            {
                var game = GetGame(summonerName);
                LastGameIdMap[summonerName] = game.GameId;
                
                GameInfos[summonerName] = null;
            }
        }

        public long? GetLastGameId(string summonerName)
        {
            if (LastGameIdMap.ContainsKey(summonerName))
            {
                return LastGameIdMap[summonerName];
            }

            return null;
        }
    }
}
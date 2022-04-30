using System.Collections.Generic;
using LeagueActivityBot.Models;

namespace LeagueActivityBot.Repository
{
    public class GameInfoInMemoryRepository
    {
        private Dictionary<string, CurrentGameInfo> GameInfos { get; }
        private Dictionary<string, long> LastGameIdMap { get; } = new Dictionary<string, long>();

        public GameInfoInMemoryRepository()
        {
            GameInfos = new Dictionary<string, CurrentGameInfo>();
        }

        public bool GameExists(string summonerName)
        {
            return GameInfos.ContainsKey(summonerName);
        }
        
        public CurrentGameInfo GetGame(string summonerName)
        {
            return GameInfos[summonerName];
        }
        
        public void AddGame(string summonerName, CurrentGameInfo gameInfo)
        {
            if (GameInfos.ContainsKey(summonerName))
            {
                GameInfos[summonerName] = gameInfo;
            }
            
            GameInfos.Add(summonerName, gameInfo);
        }
        
        public void RemoveGame(string summonerName)
        {
            if (GameInfos.ContainsKey(summonerName))
            {
                var game = GetGame(summonerName);
                LastGameIdMap.Add(summonerName, game.GameId);
                
                GameInfos.Remove(summonerName);
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
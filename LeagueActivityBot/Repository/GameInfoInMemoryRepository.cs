using System.Collections.Generic;
using LeagueActivityBot.Models;

namespace LeagueActivityBot.Repository
{
    public class GameInfoInMemoryRepository
    {
        private Dictionary<string, CurrentGameInfo> GameInfos { get; }

        public GameInfoInMemoryRepository()
        {
            GameInfos = new Dictionary<string, CurrentGameInfo>();
        }

        public bool GameExists(string summonerName)
        {
            return GameInfos.ContainsKey(summonerName);
        }
        
        public CurrentGameInfo GetLastGame(string summonerName)
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
                GameInfos.Remove(summonerName);
            }
        }
    }
}
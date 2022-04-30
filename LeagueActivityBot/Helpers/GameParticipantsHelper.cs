using System.Collections.Generic;
using System.Linq;
using LeagueActivityBot.Models;

namespace LeagueActivityBot.Helpers
{
    public class GameParticipantsHelper
    {
        private readonly IEnumerable<string> _summonerNames;
        
        public GameParticipantsHelper(IEnumerable<string> summonerNames)
        {
            _summonerNames = summonerNames;
        }

        public bool IsSoloGame(GameParticipant[] gameParticipants)
        {
            return _summonerNames.Count(n => gameParticipants.Select(s => s.SummonerName).Contains(n)) == 1;
        }
    }
}
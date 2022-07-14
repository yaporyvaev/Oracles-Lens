using System.Collections.Generic;
using System.Linq;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Models;

namespace LeagueActivityBot.Helpers
{
    public class GameParticipantsHelper
    {
        private readonly IEnumerable<Summoner> _summoners;
        
        public GameParticipantsHelper(IEnumerable<Summoner> summoners)
        {
            _summoners = summoners;
        }

        public IEnumerable<Summoner> GetSummonersInGame(SpectatorGameParticipant[] gameParticipants)
        {
            return _summoners.Where(n => gameParticipants.Select(s => s.SummonerId).Contains(n.SummonerId));
        }
    }
}
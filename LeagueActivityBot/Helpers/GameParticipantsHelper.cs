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

        public bool IsSoloGame(GameParticipant[] gameParticipants)
        {
            return _summoners.Count(n => gameParticipants.Select(s => s.SummonerName).Contains(n.Name)) == 1;
        }

        public IEnumerable<Summoner> GetSummonersInGame(GameParticipant[] gameParticipants)
        {
            return _summoners.Where(n => gameParticipants.Select(s => s.SummonerName).Contains(n.Name));
        }
    }
}
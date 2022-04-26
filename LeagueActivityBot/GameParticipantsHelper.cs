using System.Linq;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Models;

namespace LeagueActivityBot
{
    public class GameParticipantsHelper
    {
        private readonly IRepository<Summoner> _summonersRepository;

        public GameParticipantsHelper(IRepository<Summoner> summonersRepository)
        {
            _summonersRepository = summonersRepository;
        }

        public bool IsSoloGameForSummoner(string summonerName, CurrentGameInfo gameInfo)
        {
            var participantNames = gameInfo.Participants
                .Where(p => p.SummonerName != summonerName)
                .Select(s => s.SummonerName)
                .ToArray();

            return !_summonersRepository
                .GetAll()
                .Any(s => participantNames.Contains(s.Name));
        }
    }
}
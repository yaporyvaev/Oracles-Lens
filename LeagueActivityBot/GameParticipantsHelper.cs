using LeagueActivityBot.Models;
using LeagueActivityBot.Repository;

namespace LeagueActivityBot
{
    public class GameParticipantsHelper
    {
        private readonly SummonersInMemoryRepository _summonersRepository;

        public GameParticipantsHelper(SummonersInMemoryRepository summonersRepository)
        {
            _summonersRepository = summonersRepository;
        }

        public bool IsSoloGameForSummoner(string summonerName, CurrentGameInfo gameInfo)
        {
            foreach (var participant in gameInfo.Participants) 
            {
                if(participant.SummonerName == summonerName) continue;
                
                var summonerInfo = _summonersRepository.FindByName(participant.SummonerName);
                if (summonerInfo != null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Notifications.OnGameEnded;
using LeagueActivityBot.Notifications.OnSoloGameEnded;
using MediatR;

namespace LeagueActivityBot.Services
{
    public class GameService
    {
        private readonly IRepository<GameInfo> _gameInfoRepository;
        private readonly IRepository<GameParticipant> _gameParticipantRepository;
        private readonly IRiotClient _riotClient;
        private readonly IMediator _mediator;

        public GameService(IRepository<GameInfo> gameInfoRepository, IRiotClient riotClient, IMediator mediator, IRepository<GameParticipant> gameParticipantRepository)
        {
            _gameInfoRepository = gameInfoRepository;
            _riotClient = riotClient;
            _mediator = mediator;
            _gameParticipantRepository = gameParticipantRepository;
        }

        public async Task ProcessEndGame(GameInfo game)
        {
            var summoners = game.GameParticipants.Select(p => p.Summoner).ToArray();
            var matchInfo = await _riotClient.GetMatchInfo(game.GameId);
            
            if (summoners.Length > 1)
            {
                await _mediator.Publish(new OnGameEndedNotification(summoners, matchInfo));
            }
            else
            {
                await _mediator.Publish(new OnSoloGameEndedNotification(summoners.First(), matchInfo));
            }
            
            foreach (var participant in game.GameParticipants)
            {
                var matchParticipant = matchInfo.Info.Participants.First(p => p.SummonerId == participant.Summoner.SummonerId);
                participant.Assists = matchParticipant.Assists;
                participant.Deaths = matchParticipant.Deaths;
                participant.Kills = matchParticipant.Kills;
                participant.Win = matchParticipant.Win;
                participant.ChampionId = matchParticipant.ChampionId;
                participant.ChampionName = matchParticipant.ChampionName;
                participant.CreepScore = matchParticipant.TotalMinionsKilled;
                participant.PentaKills = matchParticipant.PentaKills;
                participant.VisionScore = matchParticipant.VisionScore;
                participant.DetectorWardsPlaced = matchParticipant.DetectorWardsPlaced;
                participant.FirstBloodKill = matchParticipant.FirstBloodKill;

                await _gameParticipantRepository.Update(participant);
            }

            game.IsProcessed = true;
            game.GameStartTime = matchInfo.Info.GameStartTime;
            game.GameDurationInSeconds = matchInfo.Info.GameDurationInSeconds;
            game.GameEnded = true;
            
            await _gameInfoRepository.Update(game);
        }
    }
}
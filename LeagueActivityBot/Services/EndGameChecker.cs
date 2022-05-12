using System.Linq;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Notifications.OnGameEnded;
using LeagueActivityBot.Notifications.OnSoloGameEnded;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LeagueActivityBot.Services
{
    public class EndGameChecker
    {
        private readonly IRepository<GameInfo> _gameInfoRepository;
        private readonly IRepository<Summoner> _summonerRepository;
        private readonly IRiotClient _riotClient;
        private readonly ILogger<EndGameChecker> _logger;
        private readonly IMediator _mediator;

        public EndGameChecker(
            IRepository<GameInfo> gameInfoRepository, 
            IMediator mediator,
            IRiotClient riotClient, 
            ILogger<EndGameChecker> logger, 
            IRepository<Summoner> summonerRepository)
        {
            _gameInfoRepository = gameInfoRepository;
            _mediator = mediator;
            _riotClient = riotClient;
            _logger = logger;
            _summonerRepository = summonerRepository;
        }

        public async Task Check()
        {
            var games = _gameInfoRepository.GetAll().Where(g => !g.IsProcessed).ToList();

            foreach (var game in games)
            {
                var participants = JsonConvert.DeserializeObject<string[]>(game.SummonerNamesJson);
                var participantName = participants!.First();
                var summoner = _summonerRepository.GetAll().FirstOrDefault(s => s.Name == participantName);
                
                var currentGameInfo = await _riotClient.GetCurrentGameInfo(summoner!.SummonerId);
                if(currentGameInfo.IsInGameNow) continue;

                var summoners = _summonerRepository.GetAll().Where(s => participants.Contains(s.Name)).ToArray();
                if (summoners.Length > 1)
                {
                    await _mediator.Publish(new OnGameEndedNotification(summoners, game.GameId));
                }
                else
                {
                    await _mediator.Publish(new OnSoloGameEndedNotification(summoners.First(), game.GameId));
                }

                game.IsProcessed = true;
                await _gameInfoRepository.Update(game);
            }
        }
    }
}
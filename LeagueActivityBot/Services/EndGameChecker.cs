using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Notifications.OnGameEnded;
using LeagueActivityBot.Notifications.OnSoloGameEnded;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LeagueActivityBot.Services
{
    public class EndGameChecker
    {
        private readonly IRepository<GameInfo> _gameInfoRepository;
        private readonly IRiotClient _riotClient;
        private readonly GameService _gameService;

        public EndGameChecker(
            IRepository<GameInfo> gameInfoRepository, 
            IRiotClient riotClient, GameService gameService)
        {
            _gameInfoRepository = gameInfoRepository;
            _riotClient = riotClient;
            _gameService = gameService;
        }

        public async Task Check()
        {
            var games = await _gameInfoRepository.GetAll()
                .Include(g => g.GameParticipants)
                .ThenInclude(p => p.Summoner)
                .Where(g => !g.IsProcessed)
                .ToListAsync();

            foreach (var game in games)
            {
                var summoners = game.GameParticipants.Select(p => p.Summoner);
                var currentGameInfo = await _riotClient.GetCurrentGameInfo(summoners!.First()!.SummonerId);

                if (currentGameInfo.IsInGameNow)
                {
                    if(game.GameId == currentGameInfo.GameId)
                        continue;
                }

                await _gameService.ProcessEndGame(game);
            }
        }
    }
}
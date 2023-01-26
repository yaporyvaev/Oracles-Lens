using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Models;
using Microsoft.EntityFrameworkCore;

namespace LeagueActivityBot.Services
{
    public class StatisticService
    {
        private readonly IRepository<GameInfo> _gameInfoRepository;

        public StatisticService(IRepository<GameInfo> gameInfoRepository)
        {
            _gameInfoRepository = gameInfoRepository;
        }

        /// <summary>
        /// Get games statistics
        /// </summary>
        /// <param name="daysCount">Statistics for last days count</param>
        public async Task<IEnumerable<SummonerStatistic>> GetStatistic(int? daysCount = null)
        {
            var gamesQuery = _gameInfoRepository.GetAll()
                .Include(g => g.GameParticipants)
                .ThenInclude(p => p.Summoner)
                .Where(g => g.GameEnded);

            if (daysCount.HasValue)
            {
                daysCount--;
                var filterDate = DateTime.Now.Date.AddDays(-daysCount.Value);
                gamesQuery = gamesQuery.Where(g => g.GameStartTime >= filterDate);
            }
            
            var games = await gamesQuery.ToListAsync();
            
            var statisticMap = new Dictionary<string, WinRateStatisticDto>();
            foreach (var game in games)
            {
                foreach (var gameParticipant in game.GameParticipants)
                {
                    if (!statisticMap.ContainsKey(gameParticipant.Summoner.Name))
                    {
                        statisticMap.Add(gameParticipant.Summoner.Name, new WinRateStatisticDto());
                    }

                    var winRateStatisticDto = statisticMap[gameParticipant.Summoner.Name];

                    if (gameParticipant.Win!.Value)
                        winRateStatisticDto.Wins++;
                    else 
                        winRateStatisticDto.Loses++;

                    statisticMap[gameParticipant.Summoner.Name] = winRateStatisticDto;
                }
            }

            return statisticMap.Select(m => new SummonerStatistic
            {
                SummonerName = m.Key,
                Loses = m.Value.Loses,
                Wins = m.Value.Wins
            });
        }

        private struct WinRateStatisticDto
        {
            public int Wins { get; set; }
            public int Loses { get; set; }
        }
    }
}
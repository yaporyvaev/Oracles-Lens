using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace LeagueActivityBot.Migrations
{
    public class GameParticipantsMigration
    {
        public static async Task Run(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var riotClient = scope.ServiceProvider.GetService<IRiotClient>();
            var summonerRepository = scope.ServiceProvider.GetService<IRepository<Summoner>>();
            var gameInfoRepository = scope.ServiceProvider.GetService<IRepository<GameInfo>>();
            var gameParticipantRepository = scope.ServiceProvider.GetService<IRepository<GameParticipant>>();
            
            var games = gameInfoRepository.GetAll().ToArray();
            var summonerIds = summonerRepository.GetAll().Select(s => s.SummonerId);
            
            foreach (var game in games)
            {
                var gameInfo = await riotClient.GetMatchInfo(game.GameId);
                game.GameParticipants = new List<GameParticipant>();
                
                var matchParticipants = gameInfo.Info.Participants.Where(p => summonerIds.Contains(p.SummonerId));
                foreach (var matchParticipant in matchParticipants)
                {
                    var participant = new GameParticipant
                    {
                        SummonerId = summonerRepository.GetAll()
                            .Where(s => s.SummonerId == matchParticipant.SummonerId)
                            .Select(s => s.Id)
                            .First(),
                        GameInfoId = game.Id,
                        Assists = matchParticipant.Assists,
                        Deaths = matchParticipant.Deaths,
                        Kills = matchParticipant.Kills,
                        Win = matchParticipant.Win,
                        ChampionId = matchParticipant.ChampionId,
                        ChampionName = matchParticipant.ChampionName,
                        CreepScore = matchParticipant.TotalMinionsKilled,
                        PentaKills = matchParticipant.PentaKills,
                        VisionScore = matchParticipant.VisionScore,
                        DetectorWardsPlaced = matchParticipant.DetectorWardsPlaced,
                        FirstBloodKill = matchParticipant.FirstBloodKill
                    };
                    
                    await gameParticipantRepository.Add(participant);
                }
                
                game.GameEnded = true;
                game.GameStartTime = gameInfo.Info.GameStartTime;
                game.GameDurationInSeconds = gameInfo.Info.GameDurationInSeconds;

                await gameInfoRepository.Update(game);
                
                await Task.Delay(1500);
            }
        }
    }
}
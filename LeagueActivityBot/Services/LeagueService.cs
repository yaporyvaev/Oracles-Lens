using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Constants;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Entities.Enums;
using LeagueActivityBot.Models;
using Microsoft.Extensions.Logging;
using LeagueInfo = LeagueActivityBot.Entities.LeagueInfo;

namespace LeagueActivityBot.Services
{
    public class LeagueService
    {
        private readonly IRiotClient _riotClient;
        private readonly IRepository<Summoner> _summonerRepository;
        private readonly IRepository<LeagueInfo> _leagueInfos;
        private readonly ILogger<LeagueService> _logger;

        public LeagueService(IRiotClient riotClient, IRepository<Summoner> summonerRepository, IRepository<LeagueInfo> leagueInfos, ILogger<LeagueService> logger)
        {
            _riotClient = riotClient;
            _summonerRepository = summonerRepository;
            _leagueInfos = leagueInfos;
            _logger = logger;
        }

        public async Task UpdateLeague(string summonerId)
        {
            var summoner = _summonerRepository.GetAll().FirstOrDefault(s => s.SummonerId == summonerId);
            await UpdateLeague(summoner);
        }
        
        public async Task<Dictionary<string, EndGameLeagueDelta>> GetDeltaAndUpdateLeague(IEnumerable<Summoner> summoners, long queueId)
        {
            var leagueType = queueId switch
            {
                (long) QueueType.RankedSoloDuo => LeagueType.SoloDuo,
                (long) QueueType.RankedFlex => LeagueType.Flex,
                _ => throw new InvalidOperationException()
            };

            var result = new Dictionary<string, EndGameLeagueDelta>();
            foreach (var summoner in summoners)
            {
                var (soloDuoLeague, flexLeague) = await UpdateLeague(summoner);
                var currentLeague = leagueType == LeagueType.SoloDuo ? soloDuoLeague : flexLeague;

                var delta = new EndGameLeagueDelta
                {
                    SummonerId = summoner.SummonerId,
                    CurrentLeagueInfo = currentLeague
                };

                var lastLeagueInfo = summoner.LeagueInfos.FirstOrDefault(l => l.LeagueType == leagueType);
                if (lastLeagueInfo is null || currentLeague == null)
                {
                    delta.IsAnyDelta = false;
                }
                else
                {
                    if (lastLeagueInfo.Tier == currentLeague.Tier && lastLeagueInfo.Rank == currentLeague.Rank)
                    {
                        delta.LeagueUpdated = false;
                        delta.LeaguePointsDelta = currentLeague.LeaguePoints - lastLeagueInfo.LeaguePoints;
                    }
                    else
                    {
                        delta.LeagueUpdated = true;
                    }
                }

                result.Add(summoner.SummonerId, delta);
                await Task.Delay(500); //Rate limit
            }

            return result;
        }
        
        public async Task UpdateLeague(IEnumerable<Summoner> summoners)
        {
            foreach (var summoner in summoners)
            {
                await UpdateLeague(summoner);
                await Task.Delay(500);//Rate limit delayer
            }
        }
        
        public async Task<(LeagueInfo soloDuoLeague, LeagueInfo flexLeague)> UpdateLeague(Summoner summoner)
        {
            if(summoner == null) return (null, null);

            try
            {
                var leagueInfoResponse = await _riotClient.GetLeagueInfo(summoner.SummonerId);

                var soloDuoLeague = GetLeagueInfo(summoner, leagueInfoResponse, LeagueType.SoloDuo);
                if (soloDuoLeague != null)
                {
                    await UpdateLeague(soloDuoLeague, summoner.Id);
                }
                else
                {
                    await RemoveLeague(summoner.Id, LeagueType.SoloDuo);
                }
                    
                var flexLeague = GetLeagueInfo(summoner, leagueInfoResponse, LeagueType.Flex);
                if (flexLeague != null)
                {
                    await UpdateLeague(soloDuoLeague, summoner.Id);
                }
                else
                {
                    await RemoveLeague(summoner.Id, LeagueType.Flex);
                }
                
                return (soloDuoLeague, flexLeague);
            }
            catch (Exception updateLeagueException)
            {
                _logger.LogError(updateLeagueException, "Update league failed");
            }

            return (null, null);
        }
        
        private async Task RemoveLeague(int summonerId, LeagueType leagueType)
        {
            var league = _leagueInfos.GetAll().FirstOrDefault(l => l.SummonerId == summonerId && l.LeagueType == leagueType);
            await _leagueInfos.HardRemove(league);
        }
        
        private async Task UpdateLeague(LeagueInfo leagueInfo, int summonerId)
        {
            var league = _leagueInfos.GetAll().FirstOrDefault(l => l.SummonerId == summonerId && l.LeagueType == leagueInfo.LeagueType);
            if (league != null)
            {
                var leagueId = league.Id;
                league = leagueInfo;
                league.Id = leagueId;
                    
                await _leagueInfos.Update(league);
            }
            else
            {
                await _leagueInfos.Add(leagueInfo);
            }
        }
        
        private LeagueInfo GetLeagueInfo(Summoner summoner, Models.LeagueInfo[] leagueInfoResponse, LeagueType leagueType)
        {
            var queueType = leagueType == LeagueType.SoloDuo ? QueueTypeConstants.RankedSolo : QueueTypeConstants.RankedFlex;
                
            var currentLeagueInfo = leagueInfoResponse.FirstOrDefault(l => l.QueueType == queueType);
            if (currentLeagueInfo != null)
            {
                var leagueInfo = new LeagueInfo
                {
                    LeagueType = leagueType,
                    SummonerId = summoner.Id,
                    Rank = currentLeagueInfo.GetRankIntegerRepresentation(),
                    LeaguePoints = currentLeagueInfo.LeaguePoints,
                    Tier = currentLeagueInfo.GetTierIntegerRepresentation()
                };

                return leagueInfo;
            }

            return null;
        }
    }
}
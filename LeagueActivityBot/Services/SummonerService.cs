using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Entities.Enums;
using LeagueActivityBot.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LeagueActivityBot.Services
{
    public class SummonerService
    {
        private readonly IRiotClient _riotClient;
        private readonly IRepository<Summoner> _summonerRepository;
        private readonly LeagueService _leagueService;

        public SummonerService(IRiotClient riotClient, IRepository<Summoner> summonerRepository, LeagueService leagueService)
        {
            _riotClient = riotClient;
            _summonerRepository = summonerRepository;
            _leagueService = leagueService;
        }

        public async Task<IEnumerable<Summoner>> GetSummoners()
        {
            var summoners = await _summonerRepository.GetAll()
                .ToListAsync();

            return summoners;
        } 
        
        public async Task<IEnumerable<Summoner>> GetSummonersWithLeague()
        {
            var summoners = await _summonerRepository.GetAll()
                .Include(s => s.LeagueInfos)
                .ToListAsync();

            summoners = summoners
                .OrderByDescending(s => s.LeagueInfos.FirstOrDefault(l => l.LeagueType == LeagueType.SoloDuo)?.Tier)
                .ThenBy(s => s.LeagueInfos.FirstOrDefault(l => l.LeagueType == LeagueType.SoloDuo)?.Rank)
                .ThenByDescending(s => s.LeagueInfos.FirstOrDefault(l => l.LeagueType == LeagueType.SoloDuo)?.LeaguePoints)
                .ThenByDescending(s => s.LeagueInfos.FirstOrDefault(l => l.LeagueType == LeagueType.Flex)?.Tier)
                .ThenBy(s => s.LeagueInfos.FirstOrDefault(l => l.LeagueType == LeagueType.Flex)?.Rank)
                .ThenByDescending(s => s.LeagueInfos.FirstOrDefault(l => l.LeagueType == LeagueType.Flex)?.LeaguePoints)
                .ThenBy(s => s.Name)
                .ToList();

            return summoners;
        } 

        public async Task AddSummoner(string summonerName)
        {
            var summonerInfo = await _riotClient.GetSummonerInfoByName(summonerName);
            if (summonerInfo == null) throw new ApiResponseException($"Summoner {summonerName} not found.", HttpStatusCode.NotFound);

            var summonerDto = new Summoner
            {
                SummonerId = summonerInfo.Id,
                Puuid = summonerInfo.Puuid,
                AccountId = summonerInfo.AccountId,
                Name = summonerName
            };
            
            var summonerEntity = _summonerRepository.GetAll(true)
                .FirstOrDefault(s => s.Puuid == summonerDto.Puuid);

            if (summonerEntity != null)
            {
                summonerDto.Id = summonerEntity.Id;
                await _summonerRepository.Update(summonerDto);
            }
            else
            {
                summonerEntity = await _summonerRepository.Add(summonerDto);
            }

            await _leagueService.UpdateLeague(summonerEntity);
        }

        public async Task RemoveSummoner(string summonerName)
        {
            var summoner = _summonerRepository.GetAll()
                .FirstOrDefault(s => s.Name == summonerName);

            if (summoner == null) throw new ApiResponseException($"Summoner {summonerName} not found.", HttpStatusCode.NotFound);
            
            await _summonerRepository.Remove(summoner);
        }
    }
}
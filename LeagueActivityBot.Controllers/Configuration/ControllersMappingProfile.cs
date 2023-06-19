using AutoMapper;
using LeagueActivityBot.Contracts.Game;
using LeagueActivityBot.Contracts.Summoners;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Models;

namespace LeagueActivityBot.Controllers.Configuration
{
    public class ControllersMappingProfile : Profile
    {
        public ControllersMappingProfile()
        {
            CreateMap<MatchParticipant, GameParticipantDto>();
            CreateMap<Info, GameInfoDto>();
            
            CreateMap<Summoner, SummonerDto>();
        }
    }
}
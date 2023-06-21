using System;
using AutoMapper;
using LeagueActivityBot.Models;
using LeagueActivityBot.Riot.Models.Clash;

namespace LeagueActivityBot.Riot.Configuration
{
    public class RiotMappingProfile : Profile
    {
        public RiotMappingProfile()
        {
            CreateMap<ClashScheduleRiotModel, ClashSchedule>(MemberList.Destination)
                .ForMember(d => d.RegistrationTime, opt =>
                    opt.MapFrom(s => DateTimeOffset.FromUnixTimeMilliseconds(s.RegistrationTime).LocalDateTime))
                .ForMember(d => d.StartTime, opt =>
                    opt.MapFrom(s => DateTimeOffset.FromUnixTimeMilliseconds(s.StartTime).LocalDateTime));
            
            CreateMap<ClashInfoRiotResponse, ClashInfo>(MemberList.Destination)
                .ForMember(d => d.Name, opt =>
                    opt.MapFrom(s => string.Concat(s.NameKey[0].ToString().ToUpper(), s.NameKey.Substring(1)).Replace("_", " ")))
                .ForMember(d => d.SecondaryName, opt =>
                    opt.MapFrom(s => s.NameKeySecondary.Replace("_", " ")));
        }
    }
}
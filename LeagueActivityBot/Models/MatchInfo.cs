using System;
using System.Linq;
using Newtonsoft.Json;

namespace LeagueActivityBot.Models
{
    public class MatchInfo
    {
        public Info Info { get; set; }
    }

    public class Info
    {
        public MatchParticipant[] Participants { get; set; }
        public int QueueId { get; set; }
        public long GameStartTimestamp { get; set;}
        public DateTime GameStartTime => DateTimeOffset.FromUnixTimeMilliseconds(GameStartTimestamp).LocalDateTime;
        
        [JsonProperty(PropertyName = "gameDuration")]
        public long GameDurationInSeconds { get; set;}

        public double GetTeamDamage(int teamId)
        {
            double teamDamage = Participants
                .Where(p => p.TeamId == teamId)
                .Sum(p => p.TotalDamageDealtToChampions);

            return teamDamage;
        }

        public string GetMatchDuration()
        {
            var timeSpan = TimeSpan.FromSeconds(GameDurationInSeconds);
            return $"Duration {timeSpan:mm\\:ss}.";
        }
    }

    public class MatchParticipant
    {
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public int TotalDamageDealtToChampions { get; set; }
        
        public string Puuid { get; set; }
        public string SummonerId { get; set; }
        public string SummonerName { get; set; }
        
        public string ChampionName { get; set; }
        public int ChampionId { get; set; }
        public int TeamId { get; set; }
        public bool Win { get; set; }
        public bool GameEndedInEarlySurrender { get; set; }
        public bool GameEndedInSurrender { get; set; }
        public bool FirstBloodKill { get; set; }
        
        public int TotalMinionsKilled { get; set; }
        public int NeutralMinionsKilled { get; set; }
        
        public int VisionScore { get; set; }
        public int PentaKills { get; set; }
        public int DetectorWardsPlaced { get; set; }
        public int TotalDamageTaken {get; set; }
        public int TotalHeal {get; set; }

        public string GetCreepScore() => $"{TotalMinionsKilled + NeutralMinionsKilled} CS";
        public string GetVisionScore() => $"{VisionScore} VS";
        public string GetDamageTakenScore() => $"{TotalDamageTaken:#,#} dmg taken";
        public string GetHealScore() => $"{TotalHeal:#,#} healed";

        public double Kda
        {
            get
            {
                var divider = Deaths == 0 ? 1 : Deaths;
                return (double)(Kills + Assists) / divider;
            }
        }
        public string GetScore()
        {
            return $"KDA {Kills}/{Deaths}/{Assists}";
        }
        public string GetDamage(double teamDamage)
        {
            var damagePercentage = Math.Round(TotalDamageDealtToChampions / teamDamage * 100);
            return $"{TotalDamageDealtToChampions:#,#} ({damagePercentage}%) dmg";
        }
    }
}
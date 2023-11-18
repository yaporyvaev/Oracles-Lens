using System;
using System.Linq;
using LeagueActivityBot.Entities;
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
        public long GameId { get; set; }
        public string PlatformId { get; set; }
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
        
        public int BaronKills { get; set; }
        public int DragonKills { get; set; }
        
        public int TotalDamageDealtToChampions { get; set; }
        public int TotalDamageShieldedOnTeammates { get; set; }
        public int TotalDamageTaken {get; set; }
        public int TotalHeal {get; set; }
        public int TotalHealsOnTeammates {get; set; }
        public int DamageSelfMitigated { get; set; }
        
        public int GoldEarned { get; set; }
        
        public string Puuid { get; set; }
        public string SummonerId { get; set; }
        public string SummonerName { get; set; }
        
        public string ChampionName { get; set; }
        public int ChampionId { get; set; }
        public int ChampLevel { get; set; }
        
        public int TeamId { get; set; }
        public bool Win { get; set; }
        public bool GameEndedInEarlySurrender { get; set; }
        public bool GameEndedInSurrender { get; set; }
        
        public bool FirstBloodKill { get; set; }
        public bool FirstTowerKill { get; set; }
        public bool FirstTowerAssist { get; set; }
        
        public int TotalMinionsKilled { get; set; }
        public int NeutralMinionsKilled { get; set; }
        
        public int VisionScore { get; set; }
        public int TimeCCingOthers { get; set; }
        public int PentaKills { get; set; }
        public int KillingSprees { get; set; }
        public int DetectorWardsPlaced { get; set; }

        public int Item0 { get; set; }
        public int Item1 { get; set; }
        public int Item2 { get; set; }
        public int Item3 { get; set; }
        public int Item4 { get; set; }
        public int Item5 { get; set; }
        public int Item6 { get; set; }

        public double Kda
        {
            get
            {
                var divider = Deaths == 0 ? 1 : Deaths;
                return (double)(Kills + Assists) / divider;
            }
        }

        public string GetCreepScore() => $"{TotalMinionsKilled + NeutralMinionsKilled} CS";
        public string GetVisionScore() => $"{VisionScore} VS";
        public string GetDamageTakenScore() => $"{TotalDamageTaken:#,#} dmg taken";
        public string GetHealScore() => $"{TotalHeal:#,#} healed";
        public string GetScore()
        {
            return $"KDA {Kills}/{Deaths}/{Assists}";
        }
        public string GetDamage(double teamDamage)
        {
            var damagePercentage = Math.Round(TotalDamageDealtToChampions / teamDamage * 100);
            return $"{TotalDamageDealtToChampions:#,#} ({damagePercentage}%) dmg";
        }

        public double? Score { get; set; }
        public double CalculateScore(ScoreWeights weights)
        {
            var result = 
                weights.Kda * Kda +
                weights.Gold * GoldEarned +
                weights.Level * ChampLevel +
                weights.CcTime * TimeCCingOthers +
                weights.DmgHealed * TotalHeal +
                weights.DmgMitigated * DamageSelfMitigated +
                weights.DmgShielded * TotalDamageShieldedOnTeammates +
                weights.DmgTaken * TotalDamageTaken +
                weights.DmgToChampions * TotalDamageDealtToChampions;

            return result;
        }
    }
}
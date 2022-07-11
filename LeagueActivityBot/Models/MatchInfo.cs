using System;
using System.Linq;
using System.Text;

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

        public double GetTeamDamage(int teamId)
        {
            double teamDamage = Participants
                .Where(p => p.TeamId == teamId)
                .Sum(p => p.TotalDamageDealtToChampions);

            return teamDamage;
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
        public int TeamId { get; set; }
        public bool Win { get; set; }
        public bool GameEndedInEarlySurrender { get; set; }
        public bool GameEndedInSurrender { get; set; }
        
        public int TotalMinionsKilled { get; set; }
        
        public int VisionScore { get; set; }

        public string GetCreepScore() => $"{TotalMinionsKilled} CS";
        public string GetVisionScore() => $"{VisionScore} VS";
        
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
            return $"{TotalDamageDealtToChampions.ToString($"#,#")} ({damagePercentage}%) dmg";
        }
        
        
    }
}
using System.Text;
using LeagueActivityBot.Models;

namespace LeagueActivityBot.Notifications
{
    public class BaseEndGameMessageBuilder
    {
        public static string GetRankedStat(EndGameLeagueDelta leagueDelta, bool isWin)
        {
            if (leagueDelta == null || !leagueDelta.IsAnyDelta) return string.Empty;
                
            var currentTier = LeagueInfo.GetTierStringRepresentation(leagueDelta.CurrentLeagueInfo.Tier);
            var currentRank = LeagueInfo.GetRankStringRepresentation(leagueDelta.CurrentLeagueInfo.Rank);
            
            var sb = new StringBuilder();
            if (!leagueDelta.LeagueUpdated)
            {
                if (leagueDelta.LeaguePointsDelta != 0)
                {
                    if (isWin) sb.Append("+");
                    sb.Append($"{leagueDelta.LeaguePointsDelta} LP. ");
                }
                
                sb.Append($"Current rank is {currentTier} {currentRank}, {leagueDelta.CurrentLeagueInfo.LeaguePoints} LP.");
            }
            else
            {
                sb.Append(isWin ? $"Promoted to {currentTier} {currentRank}!" : $"Demoted to {currentTier} {currentRank}.");
            }

            return sb.ToString();
        }
        
        public static string GetMatchResult(MatchParticipant participant)
        {
            if (participant.Win) return "won!";
            if (participant.GameEndedInEarlySurrender) return "FFed 15.";
            if (participant.GameEndedInSurrender) return "FFed.";
                
            return "lost.";
        }
    }
}
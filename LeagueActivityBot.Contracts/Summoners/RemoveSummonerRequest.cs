using System.ComponentModel.DataAnnotations;

namespace LeagueActivityBot.Contracts.Summoners
{
    public class RemoveSummonerRequest
    {
        [Required(ErrorMessage = "The SummonerName field is required", AllowEmptyStrings = false)]
        public string SummonerName { get; set; }
    }
}
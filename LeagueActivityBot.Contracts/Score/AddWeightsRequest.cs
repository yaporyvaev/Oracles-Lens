using System.ComponentModel.DataAnnotations;

namespace LeagueActivityBot.Contracts.Score
{
    /// <summary>
    /// Запрос на создание коэффициентов MVP
    /// </summary>
    public class AddWeightsRequest
    {
        [Required(ErrorMessage = "Kda is required")]
        public double? Kda { get; set; }
        
        [Required(ErrorMessage = "Level is required")]
        public double? Level { get; set; }
        
        [Required(ErrorMessage = "Gold is required")]
        public double? Gold { get; set; }
        
        [Required(ErrorMessage = "CcTime is required")]
        public double? CcTime { get; set; }

        [Required(ErrorMessage = "DmgToChampions is required")]
        public double? DmgToChampions { get; set; }
        
        [Required(ErrorMessage = "DmgTaken is required")]
        public double? DmgTaken { get; set; }
        
        [Required(ErrorMessage = "DmgMitigated is required")]
        public double? DmgMitigated { get; set; }

        [Required(ErrorMessage = "DmgHealed is required")]
        public double? DmgHealed { get; set; }
        
        [Required(ErrorMessage = "DmgShielded is required")]
        public double? DmgShielded { get; set; }
    }
}
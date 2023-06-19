namespace LeagueActivityBot.Contracts.Game
{
    public class GameParticipantDto
    {
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
        
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public double Kda { get; set; }

        public int BaronKills { get; set; }
        public int DragonKills { get; set; }
        
        public int TotalDamageDealtToChampions { get; set; }
        public int TotalDamageShieldedOnTeammates { get; set; }
        public int TotalDamageTaken {get; set; }
        public int TotalHeal {get; set; }
        public int TotalHealsOnTeammates {get; set; }
        public int DamageSelfMitigated { get; set; }
        
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
    }
}
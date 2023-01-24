namespace LeagueActivityBot.Constants
{
    public static class QueueTypeConstants
    {
        public const string RankedSolo = "RANKED_SOLO_5x5";
        public const string RankedFlex = "RANKED_TEAM_5x5";

        public static string GetQueueTypeById(long id)
        {
            switch (id)
            {
                case (long)QueueType.Custom: return "Custom";
                case (long)QueueType.NormalDraft: return "Normal Draft";
                case (long)QueueType.RankedSoloDuo: return "Ranked Solo\\Duo";
                case (long)QueueType.NormalBlind: return "Normal Blind";
                case (long)QueueType.RankedFlex: return "Ranked Flex";
                case (long)QueueType.ARAM: return "ARAM";
                case (long)QueueType.Clash: return "Clash";
                case (long)QueueType.BotGame1: case (long)QueueType.BotGame2: case (long)QueueType.BotGame3: return "Bot game";
                case (long)QueueType.ARURF1: case (long)QueueType.ARURF2: return "ARURF";
                case (long)QueueType.OneForAll: return "One for All";
                case (long)QueueType.NexusBlitz1: case (long)QueueType.NexusBlitz2: return "Nexus Blitz";
                case (long)QueueType.UltimateSpellBook: return "Ultimate Spellbook";
                case (long)QueueType.PickUrf: return "Pick Urf";
                default: return "Unknown game mod";
            }
        }
    }
}
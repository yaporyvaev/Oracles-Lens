namespace LeagueActivityBot.Constatnts
{
    public static class QueueType
    {
        public const string RankedSolo = "RANKED_SOLO_5x5";

        public static string GetQueueTypeById(long id)
        {
            switch (id)
            {
                case 0: return "Custom";
                case 400: return "Normal Draft";
                case 420: return "Ranked Solo\\Duo";
                case 430: return "Normal Blind";
                case 440: return "Ranked Flex";
                case 450: return "ARAM";
                case 700: return "Clash";
                case 830: case 840: case 850: return "с ботами))";
                case 900: case 1010: return "ARURF";
                case 1020: return "One for All";
                case 1200: case 1300: return "Nexus Blitz";
                case 1400: return "Ultimate Spellbook";
                case 1900: return "Pick Urf";
                default: return string.Empty;
            }
        }
    }
}
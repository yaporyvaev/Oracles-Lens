using JetBrains.Annotations;

namespace LeagueActivityBot.Riot.Models.Config
{
    [UsedImplicitly]
    public class ChampionInfoRiotModel
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public ChampionInfoImageRiotModel Image { get; set; }
    }

    [UsedImplicitly]
    public class ChampionInfoImageRiotModel
    {
        public string Full { get; set; }
        public string Sprite { get; set; }
    }
}
using System.Text;
using LeagueActivityBot.Entities;

namespace LeagueActivityBot.Helpers
{
    public static class SummonerNamesEnumerator
    {
        public static string EnumerateSummoners(Summoner[] summoners)
        {
            var namesStingBuilder = new StringBuilder();
            for (var i = 0; i < summoners.Length; i++)
            {
                var summoner = summoners[i];
                
                namesStingBuilder.Append(summoner.Name);
                
                if (i == summoners.Length - 2)
                {
                    namesStingBuilder.Append(" and ");
                    continue;
                }
                
                if (i < summoners.Length - 1)
                {
                    namesStingBuilder.Append(", ");
                }
            }

            return namesStingBuilder.ToString();
        }
    }
}
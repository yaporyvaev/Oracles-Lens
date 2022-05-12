using System.Text;
using LeagueActivityBot.Entities;

namespace LeagueActivityBot.Utils
{
    public static class SummonerNamesEnumerator
    {
        public static string EnumerateSummoners(Summoner[] summoners)
        {
            var namesStingBuilder = new StringBuilder();
            for (var i = 0; i < summoners.Length; i++)
            {
                var summoner = summoners[i];
                
                namesStingBuilder.Append(summoner.GetName());
                
                if (i <= summoners.Length - 2)
                {
                    namesStingBuilder.Append(" и ");
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
using LeagueActivityBot.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueActivityBot.Database.Configurations
{
    public class SummonerConfiguration : IEntityTypeConfiguration<Summoner>
    {
        public void Configure(EntityTypeBuilder<Summoner> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            
            builder.Property(x => x.Puuid).IsRequired().HasMaxLength(200);
            builder.Property(x => x.SummonerId).IsRequired().HasMaxLength(100);
            builder.Property(x => x.AccountId).IsRequired().HasMaxLength(100);
            
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);

            builder.Property(x => x.LeaguePoints);
            builder.Property(x => x.Rank);
            builder.Property(x => x.Tier);

            builder.HasIndex(i => i.Puuid);
            builder.HasIndex(i => i.Name);
            builder.HasIndex(i => i.SummonerId);
        }
    }
}

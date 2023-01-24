using LeagueActivityBot.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueActivityBot.Database.Configurations
{
    public class LeagueInfoConfiguration : IEntityTypeConfiguration<LeagueInfo>
    {
        public void Configure(EntityTypeBuilder<LeagueInfo> builder)
        {
            builder.Property(x => x.LeagueType).IsRequired();
            builder.Property(x => x.LeaguePoints).IsRequired();;
            builder.Property(x => x.Rank).IsRequired();;
            builder.Property(x => x.Tier).IsRequired();;
        }
    }
}
using LeagueActivityBot.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueActivityBot.Database.Configurations
{
    public class RiotSettingsConfiguration : IEntityTypeConfiguration<RiotSettings>
    {
        public void Configure(EntityTypeBuilder<RiotSettings> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.ApiKey).IsRequired().HasMaxLength(50);
        }
    }
}

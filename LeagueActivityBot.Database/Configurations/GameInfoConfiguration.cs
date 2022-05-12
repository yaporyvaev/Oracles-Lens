using LeagueActivityBot.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueActivityBot.Database.Configurations
{
    public class GameInfoConfiguration : IEntityTypeConfiguration<GameInfo>
    {
        public void Configure(EntityTypeBuilder<GameInfo> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.GameId).IsRequired();
            builder.Property(x => x.QueueId).IsRequired();
            builder.Property(x => x.GameStartTime).IsRequired();
            builder.Property(x => x.SummonerNamesJson).IsRequired().HasMaxLength(1000); 
            builder.Property(x => x.IsProcessed).IsRequired().HasDefaultValue(false);
            
            builder.HasIndex(i => i.GameId);
        }
    }
}

using LeagueActivityBot.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueActivityBot.Database.Configurations
{
    public class GameInfoConfiguration : IEntityTypeConfiguration<GameInfo>
    {
        public void Configure(EntityTypeBuilder<GameInfo> builder)
        {
            builder.Property(x => x.GameId).IsRequired();
            builder.Property(x => x.QueueId).IsRequired();
            
            builder.Property(x => x.GameStartTime).IsRequired(false);
            builder.Property(x => x.GameDurationInSeconds).IsRequired(false);
            
            builder.Property(x => x.GameEnded).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.IsProcessed).IsRequired().HasDefaultValue(false);
            
            builder.HasIndex(i => i.GameId);
        }
    }
}

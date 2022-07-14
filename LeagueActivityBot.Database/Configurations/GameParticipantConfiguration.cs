using LeagueActivityBot.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueActivityBot.Database.Configurations
{
    public class GameParticipantConfiguration : IEntityTypeConfiguration<GameParticipant>
    {
        public void Configure(EntityTypeBuilder<GameParticipant> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Win).IsRequired(false);
            builder.Property(x => x.Assists).IsRequired(false);
            builder.Property(x => x.Deaths).IsRequired(false);
            builder.Property(x => x.Kills).IsRequired(false);
            builder.Property(x => x.ChampionId).IsRequired(false);
            builder.Property(x => x.ChampionName).IsRequired(false).HasMaxLength(200);
            builder.Property(x => x.CreepScore).IsRequired(false);
            builder.Property(x => x.PentaKills).IsRequired(false);
            builder.Property(x => x.VisionScore).IsRequired(false);
            builder.Property(x => x.DetectorWardsPlaced).IsRequired(false);
            builder.Property(x => x.FirstBloodKill).IsRequired(false);

            builder.HasOne(p => p.Summoner)
                .WithMany(s => s.GameParticipants)
                .HasForeignKey(p => p.SummonerId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.HasOne(p => p.GameInfo)
                .WithMany(s => s.GameParticipants)
                .HasForeignKey(p => p.GameInfoId)              
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

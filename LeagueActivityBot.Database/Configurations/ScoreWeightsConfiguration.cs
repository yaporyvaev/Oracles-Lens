using LeagueActivityBot.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeagueActivityBot.Database.Configurations
{
    public class ScoreWeightsConfiguration : IEntityTypeConfiguration<ScoreWeights>
    {
        public void Configure(EntityTypeBuilder<ScoreWeights> builder)
        {
            builder.Property(x => x.Kda).IsRequired();
            builder.Property(x => x.Gold).IsRequired();
            builder.Property(x => x.Level).IsRequired();
            builder.Property(x => x.CcTime).IsRequired();
            builder.Property(x => x.DmgHealed).IsRequired();
            builder.Property(x => x.DmgMitigated).IsRequired();
            builder.Property(x => x.DmgShielded).IsRequired();
            builder.Property(x => x.DmgTaken).IsRequired();
            builder.Property(x => x.DmgToChampions).IsRequired();
        }
    }
}
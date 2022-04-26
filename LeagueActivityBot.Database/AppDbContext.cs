using LeagueActivityBot.Database.Configurations;
using LeagueActivityBot.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeagueActivityBot.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<RiotSettings> RiotSettings { get; set; }
        public DbSet<Summoner> Summoners { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RiotSettingsConfiguration());
        }
    }
}
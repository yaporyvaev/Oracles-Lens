using LeagueActivityBot.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeagueActivityBot.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Summoner> Summoners { get; set; }
        public DbSet<GameInfo> GameInfos { get; set; }
        public DbSet<GameParticipant> GameParticipants { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
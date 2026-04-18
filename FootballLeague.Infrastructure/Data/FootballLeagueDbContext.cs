using FootballLeague.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace FootballLeague.Infrastructure.Data
{
    public class FootballLeagueDbContext(DbContextOptions<FootballLeagueDbContext> options) : DbContext(options)
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<Match> Matches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Home Team Configuration
            modelBuilder.Entity<Match>()
            .HasOne(m => m.HomeTeam)
            .WithMany(t => t.HomeMatches)
            .HasForeignKey(m => m.HomeTeamId)
            .OnDelete(DeleteBehavior.Restrict);

            // Away Team Configuration
            modelBuilder.Entity<Match>()
                .HasOne(m => m.AwayTeam)
                .WithMany(t => t.AwayMatches)
                .HasForeignKey(m => m.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            // Add unique index on the Name property
            modelBuilder.Entity<Team>()
                .HasIndex(t => t.Name)
                .IsUnique();
        }
    }
}

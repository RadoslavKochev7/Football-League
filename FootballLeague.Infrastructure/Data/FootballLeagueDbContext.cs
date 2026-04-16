using Microsoft.EntityFrameworkCore;

namespace FootballLeague.Infrastructure.Data
{
    public class FootballLeagueDbContext(DbContextOptions<FootballLeagueDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

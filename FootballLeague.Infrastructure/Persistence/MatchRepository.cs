using FootballLeague.Core.Contracts;
using FootballLeague.Core.Entities;
using FootballLeague.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FootballLeague.Infrastructure.Persistence
{
    /// <summary>
    /// Implementation of match repository for relational database operations
    /// </summary>
    public class MatchRepository(FootballLeagueDbContext context) : IMatchRepository
    {
        public async Task AddAsync(Match match)
        {
            await context.Matches.AddAsync(match);
            await SaveChangesAsync();
        }

        public async Task<Match?> GetByIdAsync(int matchId)
        {
            return await context.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .FirstOrDefaultAsync(m => m.Id == matchId);
        }

        public async Task<IEnumerable<Match>> GetAllReadonlyAsync(Expression<Func<Match, bool>> search)
        {
            return await context.Matches
                .Where(search)
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task DeleteAsync(int matchId)
        {
            var match = await context.Matches.FindAsync(matchId);
            if (match != null)
            {
                context.Matches.Remove(match);
                await SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Match match)
        {
            context.Matches.Update(match);
            await SaveChangesAsync();
        }

        private async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
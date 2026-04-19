using FootballLeague.Core.Contracts;
using FootballLeague.Core.DTOs.Team;
using FootballLeague.Core.Entities;
using FootballLeague.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FootballLeague.Infrastructure.Persistence
{
    /// <summary>
    /// Implementation of team repository for relational database operations
    /// </summary>
    public class TeamRepository(FootballLeagueDbContext context) : ITeamRepository
    {
        public async Task AddAsync(TeamAddRequest team)
        {
            Team teamEntity = new()
            {
                Name = team.Name,
            };

            await context.Teams.AddAsync(teamEntity);  
            await SaveChangesAsync();
        }

        public async Task<bool> Any(Expression<Func<Team, bool>> search)
        {
            return await context.Teams
                .AnyAsync(search);
        }

        public async Task<Team?> GetByIdAsync(int teamId)
        {
            return await context.Teams.FindAsync(teamId);
        }

        public async Task DeleteAsync(int teamId)
        {
            Team? team = await GetByIdAsync(teamId);
            if (team != null)
            {
                context.Teams.Remove(team);
                await SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Team team)
        {
            context.Teams.Update(team);

            await SaveChangesAsync();
        }

        public async Task UpdateAsync(IEnumerable<Team> teams)
        {
            context.Teams.UpdateRange(teams);

            await SaveChangesAsync();
        }

        protected async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Team>> GetAllReadonlyAsync()
        {
            return await context.Teams
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
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
        public async Task<TeamDto> AddAsync(TeamAddRequest team)
        {
            Team teamEntity = new()
            {
                Name = team.Name,
            };

            await context.Teams.AddAsync(teamEntity);
            await SaveChangesAsync();

            return MapToDto(teamEntity);
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

        public async Task<TeamDto> UpdateAsync(Team team)
        {
            context.Teams.Update(team);

            await SaveChangesAsync();

            return MapToDto(team);
        }

        public async Task UpdateAsync(IEnumerable<Team> teams)
        {
            context.Teams.UpdateRange(teams);

            await SaveChangesAsync();
        }

        public async Task<IEnumerable<Team>> GetAllReadonlyAsync()
        {
            return await context.Teams
                .AsNoTracking()
                .ToListAsync();
        }

        private async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        private static TeamDto MapToDto(Team team)
        {
            return new TeamDto
            {
                Id = team.Id,
                Name = team.Name,
                MatchesPlayed = team.MatchesPlayed,
                Wins = team.Wins,
                Draws = team.Draws,
                Losses = team.Losses,
                GoalsFor = team.GoalsFor,
                GoalsAgainst = team.GoalsAgainst,
                Points = team.Points
            };
        }
    }
}
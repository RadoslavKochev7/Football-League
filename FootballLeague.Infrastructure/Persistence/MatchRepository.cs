using FootballLeague.Core.Contracts;
using FootballLeague.Core.DTOs.Match;
using FootballLeague.Core.Entities;
using FootballLeague.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FootballLeague.Infrastructure.Persistence
{
    /// <summary>
    /// Implementation of match repository for relational database operations
    /// </summary>
    public class MatchRepository(FootballLeagueDbContext context) : IMatchRepository
    {
        public async Task<MatchDto> AddAsync(Match match)
        {
            await context.Matches.AddAsync(match);
            await SaveChangesAsync();

            Match? addedMatch = await GetByIdAsync(match.Id);

            return MapToDto(addedMatch!);
        }

        public async Task<Match?> GetByIdAsync(int matchId)
        {
            return await context.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .FirstOrDefaultAsync(m => m.Id == matchId);
        }

        public async Task<IEnumerable<Match>> GetAllReadonlyAsync()
        {
            return await context.Matches
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

        public async Task<MatchDto?> UpdateAsync(Match match)
        {
            context.Matches.Update(match);

            await SaveChangesAsync();

            Match? updatedMatch = await GetByIdAsync(match.Id);

            return updatedMatch != null ? MapToDto(updatedMatch) : null;
        }

        private async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        private static MatchDto MapToDto(Match match)
        {
            return new MatchDto
            {
                Id = match.Id,
                HomeTeamId = match.HomeTeamId,
                HomeTeamName = match.HomeTeam?.Name,
                AwayTeamId = match.AwayTeamId,
                AwayTeamName = match.AwayTeam?.Name,
                HomeTeamGoals = match.HomeTeamGoals,
                AwayTeamGoals = match.AwayTeamGoals,
                PlayedOn = match.PlayedOn
            };
        }
    }
}
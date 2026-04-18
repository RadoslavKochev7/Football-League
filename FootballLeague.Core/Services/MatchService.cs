using FootballLeague.Core.Contracts;
using FootballLeague.Core.DTOs.Match;
using FootballLeague.Core.Entities;
using FootballLeague.Shared.Constants;

namespace FootballLeague.Core.Services
{
    /// <summary>
    /// Service for managing match operations
    /// </summary>
    public class MatchService(IMatchRepository matchRepository) : IMatchService
    {
        public async Task AddAsync(MatchCreateRequest request)
        {
            var match = new Match
            {
                HomeTeamId = request.HomeTeamId,
                AwayTeamId = request.AwayTeamId,
                HomeTeamGoals = request.HomeTeamGoals,
                AwayTeamGoals = request.AwayTeamGoals,
                PlayedOn = request.PlayedOn
            };

            await matchRepository.AddAsync(match);
        }

        public async Task DeleteAsync(int matchId)
        {
            await matchRepository.DeleteAsync(matchId);
        }

        public async Task EditAsync(int id, MatchUpdateRequest request)
        {
            var match = await matchRepository.GetByIdAsync(id);
            if (match == null)
            {
                throw new InvalidOperationException($"Match with ID {id} not found.");
            }

            match.HomeTeamGoals = request.HomeTeamGoals;
            match.AwayTeamGoals = request.AwayTeamGoals;
            match.PlayedOn = request.PlayedOn;

            await matchRepository.UpdateAsync(match);
        }

        public async Task<IEnumerable<MatchDto>> GetAllAsync()
        {
            var matches = await matchRepository.GetAllAsync();
            
            return matches.Select(m => new MatchDto
            {
                Id = m.Id,
                HomeTeamId = m.HomeTeamId,
                AwayTeamId = m.AwayTeamId,
                HomeTeamGoals = m.HomeTeamGoals,
                AwayTeamGoals = m.AwayTeamGoals,
                PlayedOn = m.PlayedOn,
                HomeTeamName = m.HomeTeam?.Name,
                AwayTeamName = m.AwayTeam?.Name
            }).ToList();
        }

        public async Task<MatchDto?> GetByIdAsync(int matchId)
        {
            Match? match = await matchRepository.GetByIdAsync(matchId);
            if (match == null)
               return null;

            return new MatchDto
            {
                Id = match.Id,
                HomeTeamId = match.HomeTeamId,
                AwayTeamId = match.AwayTeamId,
                HomeTeamGoals = match.HomeTeamGoals,
                AwayTeamGoals = match.AwayTeamGoals,
                PlayedOn = match.PlayedOn,
                HomeTeamName = match.HomeTeam?.Name,
                AwayTeamName = match.AwayTeam?.Name
            };
        }

        public async Task<bool> MatchExists(int homeTeamId, int awayTeamId)
        {
            var matches = await matchRepository.GetAllAsync();
            return matches.Any(m => 
                (m.HomeTeamId == homeTeamId && m.AwayTeamId == awayTeamId) ||
                (m.HomeTeamId == awayTeamId && m.AwayTeamId == homeTeamId));
        }

        /// <summary>
        /// Updates team statistics based on match results
        /// </summary>
        private async Task UpdateTeamStatisticsAsync(int homeTeamId, int awayTeamId, int homeGoals, int awayGoals)
        {
            
        }
    }
}

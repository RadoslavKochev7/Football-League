using FootballLeague.Core.Contracts;
using FootballLeague.Core.DTOs.Match;
using FootballLeague.Core.Entities;

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
            Match? match = await matchRepository.GetByIdAsync(id);
            if (match != null)
            {
                match.HomeTeamGoals = request.HomeTeamGoals;
                match.AwayTeamGoals = request.AwayTeamGoals;
                match.PlayedOn = request.PlayedOn;

                await matchRepository.UpdateAsync(match);
            }
        }

        public async Task<IEnumerable<MatchGetAllPlayedDto>> GetAllPlayedMatchesAsync()
        {
            IEnumerable<Match> matches = await matchRepository.GetAllReadonlyAsync(m => m.PlayedOn.HasValue);

            if (!matches.Any())
                return [];

            return matches.Select(m => new MatchGetAllPlayedDto(
                m.Id, 
                m.HomeTeam?.Name ?? string.Empty, 
                m.AwayTeam?.Name ?? string.Empty, 
                $"{m.HomeTeamGoals}-{m.AwayTeamGoals}", 
                m.PlayedOn.HasValue ? m.PlayedOn.Value.ToShortDateString() : string.Empty))
                .ToList();
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
    }
}

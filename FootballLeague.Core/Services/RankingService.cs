using FootballLeague.Core.Contracts;
using FootballLeague.Core.DTOs.Ranking;
using FootballLeague.Core.Entities;

namespace FootballLeague.Core.Services
{
    public class RankingService(ITeamRepository teamRepository) : IRankingService
    {
        public async Task<IEnumerable<RankingDto>> GetRankingAsync()
        {
            IEnumerable<Team> teams = await teamRepository.GetAllReadonlyAsync();
            return teams.Select(t => new RankingDto
            (
                TeamId: t.Id,
                TeamName: t.Name,
                PlayedMatches: t.MatchesPlayed,
                Wins: t.Wins,
                Draws: t.Draws,
                Losses: t.Losses,
                GoalsFor: t.GoalsFor,
                GoalsAgainst: t.GoalsAgainst,
                GoalDifference: t.GoalsFor - t.GoalsAgainst,
                Points: t.Points
            ))
            .OrderByDescending(r => r.Points)
            .ThenByDescending(r => r.GoalsFor)
            .ToList();
        }
    }
}

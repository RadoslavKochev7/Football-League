using FootballLeague.Core.DTOs.Match;

namespace FootballLeague.Core.Contracts
{
    public interface ITeamStatisticsService
    {
        Task AddStats(int homeTeamId, int awayTeamId, int homeGoals, int awayGoals);

        Task UpdateStats(MatchDto match, int homeGoals, int awayGoals);

        Task RevertStats(int homeTeamId, int awayTeamId, int homeGoals, int awayGoals);
    }
}

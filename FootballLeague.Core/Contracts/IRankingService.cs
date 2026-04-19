using FootballLeague.Core.DTOs.Ranking;

namespace FootballLeague.Core.Contracts
{
    public interface IRankingService
    {
        Task<IEnumerable<RankingDto>> GetRankingAsync();
    }
}

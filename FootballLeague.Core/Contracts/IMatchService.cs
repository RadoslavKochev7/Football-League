using FootballLeague.Core.DTOs.Match;

namespace FootballLeague.Core.Contracts
{
    /// <summary>
    /// Service contract for match operations
    /// </summary>
    public interface IMatchService
    {
        /// <summary>
        /// Creates a new match
        /// </summary>
        Task<MatchDto> AddAsync(MatchCreateRequest request);

        /// <summary>
        /// Updates an existing match
        /// </summary>
        Task<MatchDto?> EditAsync(int id, MatchUpdateRequest request);

        /// <summary>
        /// Deletes a match by ID
        /// </summary>
        Task DeleteAsync(int matchId);

        /// <summary>
        /// Gets a match by ID
        /// </summary>
        Task<MatchDto?> GetByIdAsync(int matchId);

        /// <summary>
        /// Gets all matches
        /// </summary>
        Task<IEnumerable<MatchGetAllPlayedDto>> GetAllPlayedMatchesAsync();
    }
}

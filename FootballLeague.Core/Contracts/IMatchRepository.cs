using FootballLeague.Core.DTOs.Match;

namespace FootballLeague.Core.Contracts
{
    /// <summary>
    /// Repository contract for Match entity operations
    /// </summary>
    public interface IMatchRepository
    {
        /// <summary>
        /// Adds a new match to the database
        /// </summary>
        Task<MatchDto> AddAsync(Entities.Match match);

        /// <summary>
        /// Gets a match by its ID
        /// </summary>
        Task<Entities.Match?> GetByIdAsync(int matchId);

        /// <summary>
        /// Gets all matches
        /// </summary>
        Task<IEnumerable<Entities.Match>> GetAllReadonlyAsync();

        /// <summary>
        /// Deletes a match by its ID
        /// </summary>
        Task DeleteAsync(int matchId);

        /// <summary>
        /// Updates an existing match
        /// </summary>
        Task<MatchDto?> UpdateAsync(Entities.Match match);
    }
}
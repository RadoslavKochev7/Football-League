using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        Task AddAsync(MatchCreateRequest request);

        /// <summary>
        /// Checks if a match already exists between two teams
        /// </summary>
        Task<bool> MatchExists(int homeTeamId, int awayTeamId);

        /// <summary>
        /// Updates an existing match
        /// </summary>
        Task EditAsync(int id, MatchUpdateRequest request);

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
        Task<IEnumerable<MatchDto>> GetAllAsync();
    }
}

using FootballLeague.Core.DTOs.Team;
using FootballLeague.Core.Entities;
using System.Linq.Expressions;

namespace FootballLeague.Core.Contracts
{
    /// <summary>
    /// Repository contract for Team entity operations
    /// </summary>
    public interface ITeamRepository
    {
        /// <summary>
        /// Adds a new team to the database
        /// </summary>
        Task<TeamDto> AddAsync(TeamAddRequest team);

        /// <summary>
        /// Checks if a team with the specified name already exists
        /// </summary>
        Task<bool> Any(Expression<Func<Team, bool>> search);

        /// <summary>
        /// Gets a team by its ID
        /// </summary>
        Task<Team?> GetByIdAsync(int teamId);


        /// <summary>
        /// Deletes a team by its ID
        /// </summary>
        Task DeleteAsync(int teamId);

        /// <summary>
        /// Updates an existing team
        /// </summary>
        Task<TeamDto> UpdateAsync(Team team);
        
        /// <summary>
        /// Updates multiple teams
        /// </summary>
        Task UpdateAsync(IEnumerable<Team> teams);

        /// <summary>
        /// Gets all teams in a read-only manner without tracking them
        /// </summary>
        Task<IEnumerable<Team>> GetAllReadonlyAsync();
    }
}
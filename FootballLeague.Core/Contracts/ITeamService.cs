using FootballLeague.Core.DTOs.Team;

namespace FootballLeague.Core.Contracts
{
    public interface ITeamService
    {
        Task AddAsync(TeamAddRequest request);

        Task<bool> TeamExists(string teamName);

        Task<bool> TeamExists(int id);

        Task EditAsync(int id, TeamUpdateRequest request);

        Task DeleteAsync(int teamId);

        Task<TeamDto?> GetByIdAsync(int teamId);
    }
}

using FootballLeague.Core.Contracts;
using FootballLeague.Core.DTOs.Team;
using FootballLeague.Core.Entities;

namespace FootballLeague.Core.Services
{
    public class TeamService(ITeamRepository teamRepository) : ITeamService
    {
        public async Task<TeamDto> AddAsync(TeamAddRequest request)
        {
            return await teamRepository.AddAsync(request);
        }

        public async Task DeleteAsync(int teamId)
        {
            await teamRepository.DeleteAsync(teamId);
        }

        public async Task<TeamDto?> EditAsync(int id, TeamUpdateRequest request)
        {
            Team? team = await teamRepository.GetByIdAsync(id);

            if (team != null)
            {
                team.Name = request.Name;

                return await teamRepository.UpdateAsync(team);
            }

            return null;
        }

        public async Task<TeamDto?> GetByIdAsync(int teamId)
        {
            Team? team = await teamRepository.GetByIdAsync(teamId);

            if (team == null)
                return null;

            return new TeamDto
            {
                Id = team.Id,
                Name = team.Name,
                MatchesPlayed = team.MatchesPlayed,
                Wins = team.Wins,
                Draws = team.Draws,
                Losses = team.Losses,
                GoalsFor = team.GoalsFor,
                GoalsAgainst = team.GoalsAgainst,
                Points = team.Points
            };
        }

        public async Task<bool> TeamExists(string teamName)
        {
            string nameTrimmed = teamName.Trim();

            return await teamRepository.Any(t => t.Name == nameTrimmed);
        }

        public async Task<bool> TeamExists(int id)
        {
            return await teamRepository.Any(t => t.Id == id);
        }
    }
}

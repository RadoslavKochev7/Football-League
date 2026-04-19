using FootballLeague.Core.Contracts;
using FootballLeague.Core.DTOs.Match;
using FootballLeague.Core.Entities;
using FootballLeague.Shared.Constants;

namespace FootballLeague.Core.Services
{
    public class TeamStatisticsService(ITeamRepository teamRepository) : ITeamStatisticsService
    {
        public async Task UpdateStats(MatchDto match, int homeGoals, int awayGoals)
        {
            await RevertStats(match.HomeTeamId, match.AwayTeamId, match.HomeTeamGoals, match.AwayTeamGoals);
            await AddStats(match.HomeTeamId, match.AwayTeamId, homeGoals, awayGoals);
        }

        public async Task RevertStats(int homeTeamId, int awayTeamId, int homeGoals, int awayGoals)
        {
            Team? homeTeam = await teamRepository.GetByIdAsync(homeTeamId);
            Team? awayTeam = await teamRepository.GetByIdAsync(awayTeamId);

            if (homeTeam == null || awayTeam == null)
                return;

            // Update matches played
            homeTeam.MatchesPlayed--;
            awayTeam.MatchesPlayed--;

            // Update goals
            homeTeam.GoalsFor -= homeGoals;
            homeTeam.GoalsAgainst -= awayGoals;
            awayTeam.GoalsFor -= awayGoals;
            awayTeam.GoalsAgainst -= homeGoals;

            // Determine match result and update points
            if (homeGoals > awayGoals)
            {
                homeTeam.Wins--;
                awayTeam.Losses--;
                homeTeam.Points -= GlobalConstants.WinPoints;
            }
            else if (awayGoals > homeGoals)
            {
                awayTeam.Wins--;
                homeTeam.Losses--;
                awayTeam.Points -= GlobalConstants.WinPoints;
            }
            else
            {
                homeTeam.Draws--;
                awayTeam.Draws--;
                homeTeam.Points -= GlobalConstants.DrawPoints;
                awayTeam.Points -= GlobalConstants.DrawPoints;
            }

            await teamRepository.UpdateAsync([homeTeam, awayTeam]);
        }

        public async Task AddStats(int homeTeamId, int awayTeamId, int homeGoals, int awayGoals)
        {
            Team? homeTeam = await teamRepository.GetByIdAsync(homeTeamId);
            Team? awayTeam = await teamRepository.GetByIdAsync(awayTeamId);

            if (homeTeam == null || awayTeam == null)
                return;

            // Update matches played
            homeTeam.MatchesPlayed++;
            awayTeam.MatchesPlayed++;

            // Update goals
            homeTeam.GoalsFor += homeGoals;
            homeTeam.GoalsAgainst += awayGoals;
            awayTeam.GoalsFor += awayGoals;
            awayTeam.GoalsAgainst += homeGoals;

            // Determine match result and update points
            if (homeGoals > awayGoals)
            {
                homeTeam.Wins++;
                awayTeam.Losses++;
                homeTeam.Points += GlobalConstants.WinPoints;
            }
            else if (awayGoals > homeGoals)
            {
                awayTeam.Wins++;
                homeTeam.Losses++;
                awayTeam.Points += GlobalConstants.WinPoints;
            }
            else
            {
                homeTeam.Draws++;
                awayTeam.Draws++;
                homeTeam.Points += GlobalConstants.DrawPoints;
                awayTeam.Points += GlobalConstants.DrawPoints;
            }

            await teamRepository.UpdateAsync([homeTeam, awayTeam]);
        }
    }
}

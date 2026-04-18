namespace FootballLeague.Core.DTOs.Match
{
    public record MatchUpdateRequest(int HomeTeamGoals, int AwayTeamGoals, DateTime? PlayedOn);
}

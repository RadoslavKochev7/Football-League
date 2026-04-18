namespace FootballLeague.Core.DTOs.Match
{
    public record MatchCreateRequest(int HomeTeamId, int AwayTeamId, int HomeTeamGoals, int AwayTeamGoals, DateTime? PlayedOn);
}

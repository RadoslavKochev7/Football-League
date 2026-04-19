namespace FootballLeague.Core.DTOs.Match
{
    public record MatchGetAllPlayedDto (int Id, string HomeTeamName, string AwayTeamName, string Score, string PlayedOn);
}

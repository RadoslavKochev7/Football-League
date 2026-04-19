namespace FootballLeague.Core.DTOs.Ranking
{
    public record RankingDto(
        int TeamId,
        string TeamName,
        int PlayedMatches,
        int Wins,
        int Draws,
        int Losses,
        int GoalsFor,
        int GoalsAgainst,
        int GoalDifference,
        int Points);
}

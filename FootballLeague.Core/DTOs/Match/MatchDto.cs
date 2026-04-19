namespace FootballLeague.Core.DTOs.Match
{
    /// <summary>
    /// DTO for match information
    /// </summary>
    public class MatchDto
    {
        public int Id { get; set; }

        public int HomeTeamId { get; set; }

        public int AwayTeamId { get; set; }

        public int HomeTeamGoals { get; set; }

        public int AwayTeamGoals { get; set; }

        public DateTime PlayedOn { get; set; }

        public string? HomeTeamName { get; set; }

        public string? AwayTeamName { get; set; }
    }
}
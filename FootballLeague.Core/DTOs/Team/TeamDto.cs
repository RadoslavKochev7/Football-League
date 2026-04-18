namespace FootballLeague.Core.DTOs.Team
{
    public class TeamDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int MatchesPlayed { get; set; }

        public int Wins { get; set; }

        public int Draws { get; set; }

        public int Losses { get; set; }

        public int GoalsFor { get; set; }

        public int GoalsAgainst { get; set; }

        public int Points { get; set; }
    }
}

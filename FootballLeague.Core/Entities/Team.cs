using FootballLeague.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Core.Entities
{
    public class Team
    {
        [Key]
        public int Id { get; init; }

        [Required]
        [StringLength(GlobalConstants.TeamNameMaxLength)]
        public required string Name { get; set; }

        public int MatchesPlayed { get; set; }

        public int Wins { get; set; }

        public int Draws { get; set; }

        public int Losses { get; set; }

        public int GoalsFor { get; set; }

        public int GoalsAgainst { get; set; }

        public int Points { get; set; }

        public virtual ICollection<Match> HomeMatches { get; set; } = [];

        public virtual ICollection<Match> AwayMatches { get; set; } = [];
    }
}

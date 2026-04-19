using FootballLeague.Shared.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballLeague.Core.Entities
{
    public class Match
    {
        [Key]
        public int Id { get; init; }

        public DateTime PlayedOn { get; set; }

        [Required]
        public int HomeTeamId { get; set; }

        [ForeignKey(nameof(HomeTeamId))]
        public virtual Team HomeTeam { get; set; } = null!;

        [Required]
        public int AwayTeamId { get; set; }

        [ForeignKey(nameof(AwayTeamId))]
        public virtual Team AwayTeam { get; set; } = null!;

        [Range(GlobalConstants.ScoredGoalsMinValue, GlobalConstants.ScoredGoalsMaxValue)]
        public int HomeTeamGoals { get; set; }

        [Range(GlobalConstants.ScoredGoalsMinValue, GlobalConstants.ScoredGoalsMaxValue)]
        public int AwayTeamGoals { get; set; }
    }
}

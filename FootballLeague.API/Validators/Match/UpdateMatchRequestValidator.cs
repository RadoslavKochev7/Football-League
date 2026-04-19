using FluentValidation;
using FootballLeague.Core.DTOs.Match;
using FootballLeague.Shared.Constants;

namespace FootballLeague.API.Validators.Match
{
    public class UpdateMatchRequestValidator : AbstractValidator<MatchUpdateRequest>
    {
        public UpdateMatchRequestValidator()
        {
            RuleFor(m => m.HomeTeamGoals)
                .InclusiveBetween(GlobalConstants.ScoredGoalsMinValue, GlobalConstants.ScoredGoalsMaxValue);
            RuleFor(m => m.AwayTeamGoals)
                .InclusiveBetween(GlobalConstants.ScoredGoalsMinValue, GlobalConstants.ScoredGoalsMaxValue);
            RuleFor(m => m.PlayedOn)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("PlayedOn date cannot be in the future.");
        }
    }
}

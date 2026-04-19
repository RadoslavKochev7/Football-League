using FluentValidation;
using FootballLeague.Core.DTOs.Match;

namespace FootballLeague.API.Validators.Match
{
    public class CreateMatchRequestValidator : AbstractValidator<MatchCreateRequest>
    {
        public CreateMatchRequestValidator()
        {
            RuleFor(m => m.HomeTeamId)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(m => m.AwayTeamId)
                .NotEmpty()
                .GreaterThan(0).WithMessage("Away team ID must be greater than 0.")
                .NotEqual(m => m.HomeTeamId).WithMessage("Home team and away team cannot be the same.");
            RuleFor(m => m.HomeTeamGoals)
                .GreaterThanOrEqualTo(0).WithMessage("Home team goals cannot be negative.");
            RuleFor(m => m.AwayTeamGoals)
                .GreaterThanOrEqualTo(0).WithMessage("Away team goals cannot be negative.");
            RuleFor(m => m.PlayedOn)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Played date cannot be in the future.")
                .When(m => m.PlayedOn.HasValue);
        }
    }
}

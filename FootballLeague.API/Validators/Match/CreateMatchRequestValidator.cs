using FluentValidation;
using FootballLeague.Core.DTOs.Match;
using FootballLeague.Shared.Constants;

namespace FootballLeague.API.Validators.Match
{
    public class CreateMatchRequestValidator : AbstractValidator<MatchCreateRequest>
    {
        public CreateMatchRequestValidator()
        {
            RuleFor(m => m.HomeTeamId)
                .NotEmpty()
                .GreaterThan(0).WithMessage(GlobalConstants.HomeTeamIdMinValueMessage);
            RuleFor(m => m.AwayTeamId)
                .NotEmpty()
                .GreaterThan(0).WithMessage(GlobalConstants.AwayTeamIdMinValueMessage)
                .NotEqual(m => m.HomeTeamId).WithMessage(GlobalConstants.HomeAndAwayTeamSameMessage);
            RuleFor(m => m.HomeTeamGoals)
                .GreaterThanOrEqualTo(0).WithMessage(GlobalConstants.HomeTeamGoalsNegativeMessage);
            RuleFor(m => m.AwayTeamGoals)
                .GreaterThanOrEqualTo(0).WithMessage(GlobalConstants.AwayTeamGoalsNegativeMessage);
            RuleFor(m => m.PlayedOn)
                .NotEmpty().WithMessage(GlobalConstants.PlayedOnDateRequiredMessage)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage(GlobalConstants.PlayedDateInFutureMessage);
        }
    }
}

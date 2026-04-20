using FluentValidation;
using FootballLeague.Core.DTOs.Team;
using FootballLeague.Shared.Constants;

namespace FootballLeague.API.Validators.Team
{
    public class CreateTeamRequestValidator : AbstractValidator<TeamAddRequest>
    {
        public CreateTeamRequestValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage(GlobalConstants.TeamNameRequiredMessage)
               .MaximumLength(GlobalConstants.TeamNameMaxLength).WithMessage(GlobalConstants.TeamNameLengthMessage);
        }
    }
}
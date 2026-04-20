using FluentValidation;
using FootballLeague.Core.DTOs.Team;
using FootballLeague.Shared.Constants;

namespace FootballLeague.API.Validators.Team
{
    public class UpdateTeamRequestValidator : AbstractValidator<TeamUpdateRequest>
    {
        public UpdateTeamRequestValidator()
        {
            RuleFor(t => t.Name)
               .NotEmpty().WithMessage(GlobalConstants.TeamNameRequiredMessage)
               .MaximumLength(GlobalConstants.TeamNameMaxLength).WithMessage(GlobalConstants.TeamNameLengthMessage);
        }
    }
}

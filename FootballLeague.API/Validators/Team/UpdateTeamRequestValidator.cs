using FluentValidation;
using FootballLeague.Core.DTOs.Team;

namespace FootballLeague.API.Validators.Team
{
    public class UpdateTeamRequestValidator : AbstractValidator<TeamUpdateRequest>
    {
        public UpdateTeamRequestValidator()
        {
            RuleFor(t => t.Name)
                .NotEmpty()
                .WithMessage("Team name is required.")
                .MaximumLength(100)
                .WithMessage("Team name must not exceed 100 characters.");
        }
    }
}

using FluentValidation;
using FootballLeague.Core.DTOs.Team;

namespace FootballLeague.API.Validators
{
    public class CreateTeamRequestValidator : AbstractValidator<TeamAddRequest>
    {
        public CreateTeamRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Team name is required.")
                .MaximumLength(100).WithMessage("Team name cannot exceed 100 characters.");
        }
    }
}
using FluentValidation;
using FluentValidation.Results;
using FootballLeague.Core.Contracts;
using FootballLeague.Core.DTOs.Team;

namespace FootballLeague.API.Endpoints
{
    public static class TeamEndpoints
    {
        public static void MapTeamEndpoints(this IEndpointRouteBuilder app)
        {
            RouteGroupBuilder group = app
                .MapGroup("/api/team")
                .WithTags("Teams")
                .WithOpenApi();

            group.MapGet("/{id:int}", async (
                int id,
                ITeamService teamService) =>
            {
                TeamDto? team = await teamService.GetByIdAsync(id);

                return team != null ? Results.Ok(team) : Results.NotFound();
            })
            .WithSummary("Get team by Id");

            group.MapPost("/", async (
                TeamAddRequest team,
                ITeamService teamService,
                IValidator<TeamAddRequest> validator) =>
            {
                ValidationResult validationResult = await validator.ValidateAsync(team);
                if (!validationResult.IsValid)
                    return Results.BadRequest(validationResult.Errors);

                if (await teamService.TeamExists(team.Name))
                    return Results.Problem($"A team with the same name already exists.");

                await teamService.AddAsync(team);

                return Results.Created();
            })
            .WithSummary("Create new team");

            group.MapPut("/{id:int}", async (
                int id,
                TeamUpdateRequest updatedTeam,
                ITeamService teamService) =>
            {
                if (!await teamService.TeamExists(id))
                    return Results.NotFound($"No team with Id {id}");

                await teamService.EditAsync(id, updatedTeam);
                return Results.Ok("Successfully updated");
            })
            .WithSummary("Update existing team");

            group.MapDelete("/{id:int}", async (
                int id,
                ITeamService teamService) =>
            {
                TeamDto? team = await teamService.GetByIdAsync(id);

                if (team == null)
                    return Results.NotFound($"No team with Id {id}");

                if (team.MatchesPlayed > 0)
                    return Results.Problem("Cannot delete a team that has played matches. You need to first delete the associated matches.");

                await teamService.DeleteAsync(id);

                return Results.NoContent();
            })
            .WithSummary("Delete an existing team");
        }
    }
}

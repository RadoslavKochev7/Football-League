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

            // GET /api/team/{id}
            // returns the team with the specified Id, or 404 if not found
            group.MapGet("/{id:int}", async (
                int id,
                ITeamService teamService) =>
            {
                TeamDto? team = await teamService.GetByIdAsync(id);

                return team != null 
                ? Results.Ok(team) 
                : Results.NotFound($"No team with Id {id}");
            })
            .WithSummary("Get team by Id");

            // POST /api/team
            // creates a new team with the provided details, returns 201 on success, or 400 if validation fails
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

            // PUT /api/team/{id}
            // updates the team with the specified Id using the provided details, returns 200 on success, 404 if team not found, or 400 if validation fails
            group.MapPut("/{id:int}", async (
                int id,
                TeamUpdateRequest updatedTeam,
                ITeamService teamService,
                IValidator<TeamUpdateRequest> validator) =>
            {
                ValidationResult validationResult = await validator.ValidateAsync(updatedTeam);
                if (!validationResult.IsValid)
                    return Results.BadRequest(validationResult.Errors);

                if (!await teamService.TeamExists(id))
                    return Results.NotFound($"No team with Id {id}");

                await teamService.EditAsync(id, updatedTeam);

                return Results.Ok("Successfully updated");
            })
            .WithSummary("Update existing team");

            // DELETE /api/team/{id}
            // deletes the team with the specified Id, returns 204 on success, 404 if team not found, or 400 if team has played matches
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

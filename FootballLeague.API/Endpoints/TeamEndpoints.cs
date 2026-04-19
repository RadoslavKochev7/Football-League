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
                : Results.NotFound();
            })
            .WithSummary("Get team by Id")
            .Produces<TeamDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

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
                    return Results.Conflict($"A team with the same name already exists.");

                TeamDto teamDto = await teamService.AddAsync(team);

                return Results.Created($"/api/team/{teamDto.Id}", teamDto);
            })
            .WithSummary("Create new team")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict);

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
                    return Results.NotFound();

                TeamDto? teamDto = await teamService.EditAsync(id, updatedTeam);

                return Results.Ok(teamDto);
            })
            .WithSummary("Update existing team")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

            // DELETE /api/team/{id}
            // deletes the team with the specified Id, returns 204 on success, 404 if team not found, or 400 if team has played matches
            group.MapDelete("/{id:int}", async (
                int id,
                ITeamService teamService) =>
            {
                TeamDto? team = await teamService.GetByIdAsync(id);

                if (team == null)
                    return Results.NotFound();

                if (team.MatchesPlayed > 0)
                    return Results.Problem(
                        detail: "Cannot delete a team that has played matches. You need to first delete the associated matches.",
                        statusCode: StatusCodes.Status409Conflict,
                        title: "Delete Conflict");

                await teamService.DeleteAsync(id);

                return Results.NoContent();
            })
            .WithSummary("Delete an existing team")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict);
        }
    }
}

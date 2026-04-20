using FluentValidation;
using FluentValidation.Results;
using FootballLeague.Core.Contracts;
using FootballLeague.Core.DTOs.Team;
using FootballLeague.Shared.Constants;

namespace FootballLeague.API.Endpoints
{
    public static class TeamEndpoints
    {
        public static void MapTeamEndpoints(this IEndpointRouteBuilder app)
        {
            RouteGroupBuilder group = app
                .MapGroup("/api/team")
                .WithTags(GlobalConstants.TeamsTag)
                .WithOpenApi();

            // GET /api/team/{id}
            group.MapGet("/{id:int}", async (
                int id,
                ITeamService teamService) =>
            {
                TeamDto? team = await teamService.GetByIdAsync(id);

                return team != null
                ? Results.Ok(team)
                : Results.NotFound();
            })
            .WithSummary(GlobalConstants.GetTeamByIdSummary)
            .Produces<TeamDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            // POST /api/team
            group.MapPost("/", async (
                TeamAddRequest team,
                ITeamService teamService,
                IValidator<TeamAddRequest> validator) =>
            {
                ValidationResult validationResult = await validator.ValidateAsync(team);
                if (!validationResult.IsValid)
                    return Results.BadRequest(validationResult.Errors);

                if (await teamService.TeamExists(team.Name))
                    return Results.Conflict(GlobalConstants.AlreadyExistingTeamValidation);

                TeamDto teamDto = await teamService.AddAsync(team);

                return Results.Created($"/api/team/{teamDto.Id}", teamDto);
            })
            .WithSummary(GlobalConstants.CreateTeamSummary)
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict);

            // PUT /api/team/{id}
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
            .WithSummary(GlobalConstants.UpdateTeamSummary)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

            // DELETE /api/team/{id}
            group.MapDelete("/{id:int}", async (
                int id,
                ITeamService teamService) =>
            {
                TeamDto? team = await teamService.GetByIdAsync(id);

                if (team == null)
                    return Results.NotFound();

                if (team.MatchesPlayed > 0)
                    return Results.Problem(
                        detail: GlobalConstants.DeleteConflictDetail,
                        statusCode: StatusCodes.Status409Conflict,
                        title: GlobalConstants.DeleteConflictTitle);

                await teamService.DeleteAsync(id);

                return Results.NoContent();
            })
            .WithSummary(GlobalConstants.DeleteTeamSummary)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict);
        }
    }
}

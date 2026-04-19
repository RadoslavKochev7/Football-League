using FluentValidation;
using FluentValidation.Results;
using FootballLeague.Core.Contracts;
using FootballLeague.Core.DTOs.Match;

namespace FootballLeague.API.Endpoints
{
    public static class MatchEndpoints
    {
        public static void MapMatchEndpoints(this IEndpointRouteBuilder app)
        {
            RouteGroupBuilder group = app
                .MapGroup("/api/match")
                .WithTags("Matches")
                .WithOpenApi();

            // GET: /api/match
            // returns all matches that have been played
            group.MapGet("/", async (IMatchService matchService) =>
            {
                IEnumerable<MatchGetAllPlayedDto> matches = await matchService.GetAllPlayedMatchesAsync();
                return Results.Ok(matches);
            })
            .WithSummary("Get all matches")
            .Produces<IEnumerable<MatchGetAllPlayedDto>>(StatusCodes.Status200OK);

            // GET: /api/match/{id}
            // returns a match by its ID, if it exists
            group.MapGet("/{id:int}", async (
                int id,
                IMatchService matchService) =>
            {
                MatchDto? match = await matchService.GetByIdAsync(id);
                if (match == null)
                    return Results.NotFound();

                return Results.Ok(match);
            })
            .WithSummary("Get a match by ID")
            .Produces<MatchDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            // POST: /api/match
            // creates a new match and updates the statistics of the involved teams
            group.MapPost("/", async (
                MatchCreateRequest match,
                IMatchService matchService,
                ITeamService teamService,
                ITeamStatisticsService statsService,
                IValidator<MatchCreateRequest> validator) =>
            {
                ValidationResult validationResult = await validator.ValidateAsync(match);
                if (!validationResult.IsValid)
                    return Results.BadRequest(validationResult.Errors);

                if (!await teamService.TeamExists(match.HomeTeamId) 
                || !await teamService.TeamExists(match.AwayTeamId))
                    return Results.NotFound();

                MatchDto matchDto = await matchService.AddAsync(match);

                await statsService.AddStats(match.HomeTeamId, match.AwayTeamId, match.HomeTeamGoals, match.AwayTeamGoals);

                return Results.Created($"/api/match/{matchDto.Id}", matchDto);
            })
            .WithSummary("Create a new match")
            .Produces<MatchDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

            // PUT: /api/match/{id}
            // updates an existing match and updates the statistics of the involved teams accordingly
            group.MapPut("/{id:int}", async (
                int id,
                MatchUpdateRequest updatedMatch,
                IMatchService matchService,
                ITeamStatisticsService statsService,
                IValidator<MatchUpdateRequest> validator) =>
            {
                ValidationResult validationResult = await validator.ValidateAsync(updatedMatch);
                if (!validationResult.IsValid)
                    return Results.BadRequest(validationResult.Errors);

                MatchDto? match = await matchService.GetByIdAsync(id);
                if (match == null)
                    return Results.NotFound();

                MatchDto? result = await matchService.EditAsync(id, updatedMatch);

                await statsService.UpdateStats(match, updatedMatch.HomeTeamGoals, updatedMatch.AwayTeamGoals);

                return Results.Ok(result);
            })
            .WithSummary("Update an existing match")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

            // DELETE: /api/match/{id}
            // deletes an existing match and reverts the statistics of the involved teams accordingly
            group.MapDelete("/{id:int}", async (
                int id,
                IMatchService matchService,
                ITeamStatisticsService statsService) =>
            {
                MatchDto? match = await matchService.GetByIdAsync(id);
                if (match == null)
                    return Results.NotFound();

                await matchService.DeleteAsync(id);

                await statsService.RevertStats(match.HomeTeamId, match.AwayTeamId, match.HomeTeamGoals, match.AwayTeamGoals);

                return Results.NoContent();
            })
            .WithSummary("Delete an existing match")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}
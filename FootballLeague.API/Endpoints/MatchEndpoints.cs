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

            group.MapGet("/", async (IMatchService matchService) =>
            {
                var matches = await matchService.GetAllAsync();
                return Results.Ok(matches);
            })
            .WithSummary("Get all matches");

            group.MapGet("/{id:int}", async (
                int id, 
                IMatchService matchService) =>
            {
                var match = await matchService.GetByIdAsync(id);

                return Results.Ok(match);
            })
            .WithSummary("Get a match by ID");

            group.MapPost("/", async (
                MatchCreateRequest match,
                IMatchService matchService,
                ITeamStatisticsService statsService,
                IValidator<MatchCreateRequest> validator) =>
            {
                ValidationResult validationResult = await validator.ValidateAsync(match);
                if (!validationResult.IsValid)
                    return Results.BadRequest(validationResult.Errors);

                await matchService.AddAsync(match);
                await statsService.AddStats(match.HomeTeamId, match.AwayTeamId, match.HomeTeamGoals, match.AwayTeamGoals);

                return Results.Created();
            })
            .WithSummary("Create a new match");

            group.MapPut("/{id:int}", async (
                int id, 
                MatchUpdateRequest updatedMatch, 
                IMatchService matchService,
                ITeamStatisticsService statsService) =>
            {
                var match = await matchService.GetByIdAsync(id);
                if (match == null)
                    return Results.NotFound($"No match with Id {id}");

                //await matchService.EditAsync(updatedMatch);
                await statsService.UpdateStats(match, updatedMatch.HomeTeamGoals, updatedMatch.AwayTeamGoals);
                return Results.Ok("Successfully updated");
            })
            .WithSummary("Update an existing match");

            group.MapDelete("/{id:int}", async (
                int id, 
                IMatchService matchService) =>
            {
                await matchService.DeleteAsync(id);
                return Results.NoContent();
            })
            .WithSummary("Delete an existing match");
        }
    }
}
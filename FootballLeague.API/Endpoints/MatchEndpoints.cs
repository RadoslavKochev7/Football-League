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
            .WithSummary("Get all matches");

            // GET: /api/match/{id}
            // returns a match by its ID, if it exists
            group.MapGet("/{id:int}", async (
                int id,
                IMatchService matchService) =>
            {
                var match = await matchService.GetByIdAsync(id);
                if (match == null)
                    return Results.NotFound($"No match with Id {id}");

                return Results.Ok(match);
            })
            .WithSummary("Get a match by ID");

            // POST: /api/match
            // creates a new match and updates the statistics of the involved teams
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

                // Update team statistics only if the match has been played (i.e., PlayedOn is not null)
                if (match.PlayedOn.HasValue)
                    await statsService.AddStats(match.HomeTeamId, match.AwayTeamId, match.HomeTeamGoals, match.AwayTeamGoals);

                return Results.Created();
            })
            .WithSummary("Create a new match");

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
                    return Results.NotFound($"No match with Id {id}");

                await matchService.EditAsync(id, updatedMatch);

                await UpdateTeamStatsIfNecessary(match, updatedMatch, statsService);

                return Results.Ok();
            })
            .WithSummary("Update an existing match");

            // DELETE: /api/match/{id}
            // deletes an existing match and reverts the statistics of the involved teams accordingly
            group.MapDelete("/{id:int}", async (
                int id,
                IMatchService matchService,
                ITeamStatisticsService statsService) =>
            {
                MatchDto? match = await matchService.GetByIdAsync(id);
                if (match == null)
                    return Results.NotFound($"No match with Id {id}");

                await matchService.DeleteAsync(id);

                if (match.PlayedOn.HasValue)
                    await statsService.RevertStats(match.HomeTeamId, match.AwayTeamId, match.HomeTeamGoals, match.AwayTeamGoals);

                return Results.NoContent();
            })
            .WithSummary("Delete an existing match");
        }

        private static async Task UpdateTeamStatsIfNecessary(
            MatchDto originalMatch,
            MatchUpdateRequest updatedMatch,
            ITeamStatisticsService statsService)
        {
            if (originalMatch.PlayedOn.HasValue && updatedMatch.PlayedOn.HasValue)
                await statsService.UpdateStats(originalMatch, updatedMatch.HomeTeamGoals, updatedMatch.AwayTeamGoals);
            else if (!originalMatch.PlayedOn.HasValue && updatedMatch.PlayedOn.HasValue)
                await statsService.AddStats(originalMatch.HomeTeamId, originalMatch.AwayTeamId, updatedMatch.HomeTeamGoals, updatedMatch.AwayTeamGoals);
            else if (originalMatch.PlayedOn.HasValue && !updatedMatch.PlayedOn.HasValue)
                await statsService.RevertStats(originalMatch.HomeTeamId, originalMatch.AwayTeamId, originalMatch.HomeTeamGoals, originalMatch.AwayTeamGoals);
        }
    }
}
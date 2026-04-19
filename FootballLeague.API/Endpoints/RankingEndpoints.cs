using FootballLeague.Core.Contracts;
using FootballLeague.Core.DTOs.Ranking;

namespace FootballLeague.API.Endpoints
{
    public static class RankingEndpoints
    {
        public static void MapRankingEndpoints(this IEndpointRouteBuilder app)
        {
            // GET: /api/ranking
            // Retrieves the current ranking of all teams in the league.
            app.MapGet("/api/ranking", async (IRankingService rankingService) =>
            {
                IEnumerable<RankingDto> ranking = await rankingService.GetRankingAsync();
                return Results.Ok(ranking);
            })
            .WithTags("Ranking")
            .WithSummary("Gets the current ranking of all teams.");
        }
    }
}

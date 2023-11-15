using Microsoft.AspNetCore.Http.HttpResults;

using Cloud5.Queries;

namespace Cloud5.Endpoints;

public class StatsEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/stats/player/{fullName}", PlayerStats)
           .Produces<Ok<PlayerStats>>()
           .Produces<NotFound>();
    }
    
    private static async Task<IResult> PlayerStats(
        string? fullName,
        GetPlayerStats query,
        ILogger<StatsEndpoints> logger,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation("Getting stats for player {Name}", fullName);
        
        var stats = await query.Execute(fullName, cancellationToken);
        return stats is null ? Results.NotFound("Player not found") : Results.Ok(stats);
    }
}
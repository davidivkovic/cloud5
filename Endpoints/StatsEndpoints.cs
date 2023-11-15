using Microsoft.AspNetCore.Http.HttpResults;

using Cloud5.Queries;

namespace Cloud5.Endpoints;

public class StatsEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/stats/{name}", PlayerStats)
           .Produces<Ok<PlayerStats>>()
           .Produces<NotFound>();
    }
    
    private static async Task<IResult> PlayerStats(
        string? name,
        GetPlayerStats query,
        ILogger<StatsEndpoints> logger,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation("Getting stats for player {Name}", name);
        
        var stats = await query.Execute(name, cancellationToken);
        return stats is null ? Results.NotFound("Player not found") : Results.Ok(stats);
    }
}
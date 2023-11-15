using Cloud5.Data;

namespace Cloud5.Services;

// BackgroundService is perfect if the requirements change to processing
// stream events in a loop (eg. Kafka or SQS), that may require scoped services

/// <summary>
/// This service runs automatically when the application starts.
/// Reads player data from a CSV file and saves it to the database.
/// The file path can be specified using the environment variable <b>CSV_PLAYER_DATA</b>.
/// The default path is <b>./Data/Players.csv</b>
/// </summary>
public class PlayerProcessor : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<PlayerProcessor> _logger;

    public PlayerProcessor(IServiceProvider services, ILogger<PlayerProcessor> logger)
    {
        _services = services;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var path = Environment.GetEnvironmentVariable("CSV_PLAYER_DATA") ?? "./Data/Players.csv";
        var players = CsvPlayerParser.Parse(path);
        
        await using var scope = _services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        _logger.LogInformation("Saving {Count} players to database", players.Count);
        
        dbContext.AddRange(players);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
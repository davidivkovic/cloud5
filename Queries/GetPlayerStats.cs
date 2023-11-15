using Microsoft.EntityFrameworkCore;

using Cloud5.Data;

namespace Cloud5.Queries;

public class GetPlayerStats
{
    private readonly AppDbContext _dbContext;
    
    public GetPlayerStats(AppDbContext dbContext) => _dbContext = dbContext;
    
    public Task<PlayerStats?> Execute(string? playerName, CancellationToken cancellationToken)
    {
        return _dbContext.Players
            .AsNoTracking()
            .Where(player => player.Name == playerName)
            .Select(player => new PlayerStats(player))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
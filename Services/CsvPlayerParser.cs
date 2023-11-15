using Cloud5.Model;

namespace Cloud5.Services;

public static class CsvPlayerParser
{
    private const int NumberOfColumns = 13;
    
    private static string[] GetLinesFromFile(string filePath)
    {
        try
        {
            return File.ReadAllLines(filePath);
        }
        catch
        {
            return Array.Empty<string>();
        }
    }
    
    /// <summary>Parses a CSV file into a list of players</summary>
    /// <remarks>
    /// The file must have the following ordered columns: PLAYER, POSITION, FTM, FTA, 2PM, 2PA, 3PM, 3PA, REB, BLK, AST, STL, TOV.
    /// The POSITION must be one of the following: C, PF, SF, SG, PG.
    /// </remarks>
    /// <param name="filePath">The relative path to the file to be parsed</param>
    /// <returns>A list of players or an empty list if the file does not exist or is empty</returns>
    public static List<Player> Parse(string filePath)
    {
        var lines = GetLinesFromFile(filePath);
        return ParseLines(lines);
    }
    
    /// <summary>Parses an array of CSV lines into a list of players.</summary>
    /// <remarks>
    /// Each line must have the following ordered columns: PLAYER, POSITION, FTM, FTA, 2PM, 2PA, 3PM, 3PA, REB, BLK, AST, STL, TOV.
    /// The POSITION must be one of the following: C, PF, SF, SG, PG.
    /// The first line (header) must be provided if <paramref name="skipHeader"/> is set to true (default).
    /// </remarks>
    /// <param name="lines">The CSV lines to be parsed.</param>
    /// <param name="skipHeader">Whether to skip the first line (header) or not.</param>
    /// <returns>A list of players or an empty list if the file does not exist or is empty.</returns>
    public static List<Player> ParseLines(string[] lines, bool skipHeader = true)
    {
        var players = new Dictionary<string, Player>();
        // Skip the first line (header) if required and the file is not empty
        var start = lines.Length > 0 && skipHeader ? 1 : 0;
        
        foreach (var line in lines[start..])
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            
            var values = line.Split(',');
            if (values.Length != NumberOfColumns) continue;
            
            var player = new Player
            {
                Name = values[0].Trim(),
                Position = ParsePosition(values[1]),
                FreeThrowsMade = ParseInt(values[2]),
                FreeThrowsAttempted = ParseInt(values[3]),
                TwoPointsMade = ParseInt(values[4]),
                TwoPointsAttempted = ParseInt(values[5]),
                ThreePointsMade = ParseInt(values[6]),
                ThreePointsAttempted = ParseInt(values[7]),
                Rebounds = ParseInt(values[8]),
                Blocks = ParseInt(values[9]),
                Assists = ParseInt(values[10]),
                Steals = ParseInt(values[11]),
                Turnovers = ParseInt(values[12]),
                GamesPlayed = 1
            };
            
            if (!players.TryAdd(player.Name, player))
            {
                players[player.Name] += player;
            }
        }
        
        return players.Values.ToList();
    }

    // Safely parse PlayerPosition enum
    private static PlayerPosition ParsePosition(string? value) =>
        Enum.TryParse<PlayerPosition>(value?.Trim(), out var position) ? position : default;
    
    // Since the data is not validated, we need to make sure the values are positive numbers
    private static int ParseInt(string? value) => int.TryParse(value?.Trim(), out var number) ? Math.Abs(number) : 0;
}
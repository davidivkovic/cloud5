using Cloud5.Model;

namespace Cloud5.Queries;

public record ShootingStats(double Attempts, double Made, double ShootingPercentage);

public record TraditionalStats(
    ShootingStats FreeThrows,
    ShootingStats TwoPoints,
    ShootingStats ThreePoints,
    double Points,
    double Rebounds,
    double Blocks,
    double Assists,
    double Steals,
    double Turnovers
);

public record AdvancedStats(
    double Valorization,
    double EffectiveFieldGoalPercentage,
    double TrueShootingPercentage,
    double HollingerAssistRatio
);

/// <summary>
/// Represents the <see cref="Traditional"/> and <see cref="Advanced"/>
/// stats for all the games of a player combined.
/// The contained percentages are rounded to one decimal point.
/// All other stats are averaged per game and rounded as well.
/// </summary>
public class PlayerStats
{
    public string PlayerName { get; init; }
    public int GamesPlayed { get; init; }
    public TraditionalStats Traditional { get; init; }
    public AdvancedStats Advanced { get; init; }

    public PlayerStats(Player player)
    {
        PlayerName = player.Name;
        GamesPlayed = player.GamesPlayed;
        Traditional = new TraditionalStats(
            new ShootingStats(
                Average(player.FreeThrowsAttempted),
                Average(player.FreeThrowsMade),
                Rounded(player.FreeThrowPercentage)
            ),
            new ShootingStats(
                Average(player.TwoPointsAttempted),
                Average(player.TwoPointsMade),
                Rounded(player.TwoPointsPercentage)
            ),
            new ShootingStats(
                Average(player.ThreePointsAttempted),
                Average(player.ThreePointsMade),
                Rounded(player.ThreePointsPercentage)
            ),
            Average(player.Points),
            Average(player.Rebounds),
            Average(player.Blocks),
            Average(player.Assists),
            Average(player.Steals),
            Average(player.Turnovers)
        );
        Advanced = new AdvancedStats(
            Average(player.Valorization),
            Rounded(player.EffectiveFieldGoalPercentage),
            Rounded(player.TrueShootingPercentage),
            Rounded(player.HollingerAssistRatio)
        );
    }

    private double Rounded(double value) => Math.Round(value, 1);
    private double Average(double value) => Rounded(value / GamesPlayed);
};
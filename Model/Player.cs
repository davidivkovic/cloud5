namespace Cloud5.Model;

public enum PlayerPosition
{
    PG,
    SG,
    SF,
    PF,
    C
}

public class Player
{
    public string Name { get; init; }
    public PlayerPosition Position { get; init; }
    public int FreeThrowsMade { get; set; }
    public int FreeThrowsAttempted { get; set; }
    public int TwoPointsMade { get; set; }
    public int TwoPointsAttempted { get; set; }
    public int ThreePointsMade { get; set; }
    public int ThreePointsAttempted { get; set; }
    public int Rebounds { get; set; }
    public int Blocks { get; set; }
    public int Assists { get; set; }
    public int Steals { get; set; }
    public int Turnovers { get; set; }
    public int GamesPlayed { get; set; }

    public int Points => FreeThrowsMade + 2 * TwoPointsMade + 3 * ThreePointsMade;
    public double FreeThrowPercentage => Percentage(FreeThrowsMade, FreeThrowsAttempted);
    public double TwoPointsPercentage => Percentage(TwoPointsMade, TwoPointsAttempted);
    public double ThreePointsPercentage => Percentage(ThreePointsMade, ThreePointsAttempted);

    public double Valorization => Points + Rebounds + Blocks + Assists + Steals -
        (FreeThrowsAttempted - FreeThrowsMade + TwoPointsAttempted - TwoPointsMade +
        ThreePointsAttempted - ThreePointsMade + Turnovers);

    public double EffectiveFieldGoalPercentage => Percentage(TwoPointsMade + ThreePointsMade + 0.5 * ThreePointsMade,
        TwoPointsAttempted + ThreePointsAttempted);

    public double TrueShootingPercentage =>
        Percentage(Points, 2 * (TwoPointsAttempted + ThreePointsAttempted + 0.475 * FreeThrowsAttempted));

    public double HollingerAssistRatio => Percentage(Assists,
        TwoPointsAttempted + ThreePointsAttempted + 0.475 * FreeThrowsAttempted + Assists + Turnovers);
    
    public static Player operator +(Player a, Player b)
    {
        a.FreeThrowsMade += b.FreeThrowsMade;
        a.FreeThrowsAttempted += b.FreeThrowsAttempted;
        a.TwoPointsMade += b.TwoPointsMade;
        a.TwoPointsAttempted += b.TwoPointsAttempted;
        a.ThreePointsMade += b.ThreePointsMade;
        a.ThreePointsAttempted += b.ThreePointsAttempted;
        a.Rebounds += b.Rebounds;
        a.Blocks += b.Blocks;
        a.Assists += b.Assists;
        a.Steals += b.Steals;
        a.Turnovers += b.Turnovers;
        a.GamesPlayed += b.GamesPlayed;
        
        return a;
    }

    private static double Percentage(double a, double b) => Ratio(a, b) * 100;
    private static double Ratio(double a, double b) => b != 0 ? a / b : 0;
}
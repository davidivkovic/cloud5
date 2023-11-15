using Cloud5.Queries;
using Cloud5.Services;
using NUnit.Framework;

namespace Cloud5.Tests;

[TestFixture]
public class CsvPlayerParserTests
{
    [Test]
    public void ParseLinesWithValidCsvReturnsPlayersWithStats()
    {
        var csvLines = """
           PLAYER,POSITION,FTM,FTA,2PM,2PA,3PM,3PA,REB,BLK,AST,STL,TOV
           Luyanda Yohance,PG,6,6,0,4,4,4,1,1,4,2,2
           Jaysee Nkrumah,PF,0,4,2,2,2,4,2,1,2,0,1
           Nkosinathi Cyprian,SF,2,6,3,9,0,4,4,2,1,0,2
           Sifiso Abdalla,PF,7,7,2,6,1,5,1,1,0,3,2
           Jaysee Nkrumah,PF,0,5,5,5,3,6,1,1,0,3,2
           Nkosinathi Cyprian,SF,1,3,2,6,0,2,7,1,1,0,1
           Jaysee Nkrumah,PF,0,4,4,4,1,2,2,0,0,2,1
        """.Split("\n");

        var players = CsvPlayerParser.ParseLines(csvLines);
        Assert.AreEqual(4, players.Count);
        
        var playerStats = new PlayerStats(players[1]);
        Assert.AreEqual("Jaysee Nkrumah", playerStats.PlayerName, "Player name");
        Assert.AreEqual(3, playerStats.GamesPlayed, "Games played");
        
        var traditional = new TraditionalStats(
            new ShootingStats(4.3, 0, 0),
            new ShootingStats(3.7, 3.7, 100),
            new ShootingStats(4, 2, 50),
            13.3,
            1.7,
            0.7,
            0.7,
            1.7,
            1.3
        );
        
        // Using auto generated structural equality of the record types
        Assert.AreEqual(traditional, playerStats.Traditional, "Traditional stats");
        
        var advanced = new AdvancedStats(10.3, 87, 68.6, 5.7);
        Assert.AreEqual(advanced, playerStats.Advanced, "Advanced stats");
    }

    [Test]
    public void ParseLinesWithMalformedCsvReturnsNoPlayers()
    {
        var csvLines = """
           PLAYER,POSITION,FTM,FTA,2PM,2PA,3PM,3PA,REB,BLK,AST,STL,TOV
           John,PG,GS,6,6,0,4,4, ,1,1,4,2,2
           @)$@_==-R3KL,PF,DT,0,D
           FO3OK3,PF,7,@*(7,2,6,1,5,1,1,3w.t,,0,3,2
           Nkrumah,PF,0,5,,5,5^#,3,6,1,1,0,323,.5,2
           Nkosinathi Cyprian,SF,1<#^,3,2,6,0,2,7,12,35,,1,0,1
        """.Split("\n");

        var players = CsvPlayerParser.ParseLines(csvLines);

        Assert.AreEqual(0, players.Count, "Number of players");
    }
    
    [Test]
    public void ParseLinesWithEmptyCsvReturnsNoPlayers()
    {
        var csvLines = "".Split("\n");

        var players = CsvPlayerParser.ParseLines(csvLines);

        Assert.AreEqual(0, players.Count, "Number of players");
    }

    [Test]
    public void ParseExistentValidCsvFileReturnsPlayers()
    {
        const string filePath = "./Data/Players.csv";
        var players = CsvPlayerParser.Parse(filePath);
        
        Assert.Greater(players.Count, 0, "Number of players");
    }
    
    [Test]
    public void ParseNonExistentCsvFileReturnsNoPlayers()
    {
        const string filePath = "./Data/08qz9wu1a40sic.csv";
        var players = CsvPlayerParser.Parse(filePath);
        
        Assert.AreEqual(0, players.Count, "Number of players");
    }
}
using Cloud5.Queries;
using Cloud5.Services;
using NUnit.Framework;

namespace Cloud5.Tests;

[TestFixture]
public class CsvPlayerParserTests
{
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void Test1()
    {
        var csvLines = """
            PLAYER,POSITION,FTM,FTA,2PM,2PA,3PM,3PA,REB,BLK,AST,STL,TOV
            Luyanda Yohance,PG,6,6,0,4,4,4,1,1,4,2,2
            Jaysee Nkrumah,PF,0,4,2,2,2,4,2,1,2,0,1
            Nkosinathi Cyprian,SF,2,6,3,9,0,4,4,2,1,0,2
            Sifiso Abdalla,PF,7,7,2,6,1,5,1,1,0,3,2
            Jaysee Nkrumah,PF,0,5,5,5,3,6,1,1,0,3,2
            Nkosinathi Cyprian,SF,1,3,2,6,0,2,7,1,1,0,1
        """.Split("\n");

        var players = CsvPlayerParser.ParseLines(csvLines);
        var playerStats = new PlayerStats(players[1]);
        
        Assert.AreEqual(4, players.Count);        
        
        Assert.AreEqual("Jaysee Nkrumah", playerStats.PlayerName, "Player name");
        Assert.AreEqual(2, playerStats.GamesPlayed, "Games played");
        Assert.AreEqual(0, playerStats.Traditional.FreeThrows.Made, "Free throws made");
        Assert.AreEqual(4.5, playerStats.Traditional.FreeThrows.Attempts, "Free throws attempted");
        Assert.AreEqual(3.5, playerStats.Traditional.TwoPoints.Made, "Two points made");
    }
}
# Cloud5
A Levi9 cloud hackathon. This application aggregates the match data of basketball players and calculates their statistics.

## Built using 🔧

- &nbsp; <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRxo1QGx_G_1-2qBwh3RMPocLoKxD782w333Q&usqp=CAU" align="center" width="24" height="24"/> <a href="https://dotnet.microsoft.com/en-us/apps/aspnet">&nbsp; ASP.NET 7 + Entity Framework 7 </a>
- <img src="https://user-images.githubusercontent.com/25181517/117207330-263ba280-adf4-11eb-9b97-0ac5b40bc3be.png" align="center" width="40" height="34"/><a href="https://www.docker.com/"> Docker </a>

## Installation options
1. The easiest way to get started is using the provided Docker image.
2. Alternatively, you can run the app using the `dotnet` CLI. You can download and install the dotnet SDK 7.0 for your operating system [here](https://dotnet.microsoft.com/en-us/download/dotnet/7.0). 
In case you already have it installed but are not sure of the version, run the 
`dotnet --list-sdks` command in your terminal to check.

## How to run⚡

### Clone the project 
   - `git clone https://github.com/davidivkovic/cloud5.git`
   - `cd cloud5`
   

### Method 1: Docker
- Build and tag the provided image: `docker build . -t davidivkovic/cloud5`
- Run the container on port **5180**: `docker run -p 5180:8080 davidivkovic/cloud5`

_(Optional)_ To set your own CSV file containing player data, mount it as a volume at `/Data/Players.csv` on startup. For example:
- `docker run -v ~/Downloads/MyPlayerData.csv:/Data/Players.csv -p 5180:8080 davidivkovic/cloud5`

**Note**: If you are using Windows adjust the source path accordingly (eg. `C:/Downloads/MyPlayerData.csv`)

### Method 2: Dotnet CLI
   
 - Run the app on port **5180** `dotnet run`

_(Optional)_ To set your own CSV file containing player data, simply replace the contents of the `/Data/Players.csv` file with your own.

_(Optional)_ Another way to do this is by setting the `CSV_PLAYER_DATA` environment variable to the path of
your file. For example:
- Windows Powershell: `$env:CSV_PLAYER_DATA='C:/Downloads/MyPlayerData.csv'; dotnet run`
- Linux Bash: `export CSV_PLAYER_DATA=~/Downloads/MyPlayerData.csv; dotnet run`

## Data Format

The format of the data to provide in the CSV files is given in the following example:
```
PLAYER,POSITION,FTM,FTA,2PM,2PA,3PM,3PA,REB,BLK,AST,STL,TOV
Luyanda Yohance,PG,6,6,0,4,4,4,1,1,4,2,2
Jaysee Nkrumah,PF,0,4,2,2,2,4,2,1,2,0,1
Nkosinathi Cyprian,SF,2,6,3,9,0,4,4,2,1,0,2
```

## API

The application will be available on `http://localhost:5180` with a single endpoint `/stats/player/{playerFullName}` that supports the `GET HTTP` method.

An example request for the player _Sifiso Abdalla_, using the provided CSV data loaded on application startup:

```http request
GET http://localhost:5180/stats/player/Sifiso Abdalla
```
```json
HTTP/1.1 200 OK
Content-Type: application/json
        
{
  "playerName": "Sifiso Abdalla",
  "gamesPlayed": 3,
  "traditional": {
    "freeThrows": {
      "attempts": 4.7,
      "made": 3.3,
      "shootingPercentage": 71.4
    },
    "twoPoints": {
      "attempts": 4.7,
      "made": 3,
      "shootingPercentage": 64.3
    },
    "threePoints": {
      "attempts": 6.3,
      "made": 1,
      "shootingPercentage": 15.8
    },
    "points": 12.3,
    "rebounds": 5.7,
    "blocks": 1.7,
    "assists": 0.7,
    "steals": 1,
    "turnovers": 1.3
  },
  "advanced": {
    "valorization": 11.7,
    "effectiveFieldGoalPercentage": 40.9,
    "trueShootingPercentage": 46.7,
    "hollingerAssistRatio": 4.4
  }
}
```

An example request for a player that doesn't exist, returning `HTTP 404`:
```http request
GET http://localhost:5180/stats/player/Sifiso Abdalla
```
```json
HTTP/1.1 404 Not Found
Content-Type: text/plain

"Player not found"
```

## Testing

The `dotnet` CLI is required in order to run the unit tests. 

Run the `dotnet test` command in your terminal to get the test report.

```
Passed!  - Failed:     0, Passed:     1, Skipped:     0, Total:     1, Duration: 19 ms - Cloud5.dll (net7.0)
```
using GameAPI.Data;
using GameAPI.Models;
using GameAPI.Patterns;
using GameAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GameAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly GameDbContext _db;
        private readonly GameBrainService _gameBrain;
        private string[] args;
        private readonly IConfiguration _config;

        public GamesController(GameDbContext db, GameBrainService gameBrain, IConfiguration config)
        {
            _db = db;
            _gameBrain = gameBrain;
            _config = config;
        }
        public GameDTO convertGame(Game game)
        {
            var gameDTO = new GameDTO
            {
                Id = game.Id,
                Name = game.Name ?? "Unknown",

                Description = game.Description,

                //Rating = game.Rating,
                Rating = game.Rating,


                RatingCount = game.RatingCount,

                ImageUrl = game.ImageUrl,

                GameBrainUrl = game.GameBrainUrl,

                Platforms = game.Platforms.Trim().Split("|", StringSplitOptions.TrimEntries),

                Genres = game.Genres,

                Screenshots = game.Screenshots?.Trim().Split("|", StringSplitOptions.TrimEntries),

                CachedAt = DateTime.UtcNow
            };

            return gameDTO;

        }

        public List<GameDTO> fetchGamesDTO(List<Game> games)
        {
            
            List<GameDTO> gamesList = [];


            foreach (var game in games)
            {
                var gameDTO = convertGame(game);

                gamesList.Add(gameDTO);
                Console.WriteLine(gameDTO.Platforms);

            }
            return gamesList;
        }

        // GET: api/games
        [HttpGet]
        public async Task<ActionResult<List<GameDTO>>> GetGames(string? genre, string? platform, string? sortOrder, string? searchText)
        {
            var gamequery = _db.Games.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(platform))
            {
                gamequery = gamequery.Where(m => m.Platforms.Contains(platform));
            }

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                gamequery = gamequery.Where(m => m.Name.Contains(searchText));
            }

            // genre lookup
            if (!string.IsNullOrWhiteSpace(genre))
            {
                gamequery = gamequery.Where(m => m.Genres==genre);
            }

            // sorting 
            if(!string.IsNullOrWhiteSpace(sortOrder))
            {
                if (sortOrder== "-name")
                {
                    gamequery = gamequery.OrderBy(m => m.Name);
                }
                else if (sortOrder == "-released")
                {
                    gamequery = gamequery.OrderBy(m => m.CachedAt);
                }
                else 
                {
                    gamequery = gamequery.OrderBy(m => m.Rating);
                }

            }

                var games = await gamequery.ToListAsync();
            if(games==null)
            {
                //return Result<List<GameDTO>>.Failure("Game not found");
                return BadRequest("No games found");
            }
                var gamesList = fetchGamesDTO(games);

                //return Result<List<GameDTO>>.Success(gamesList);
                return Ok(gamesList);
            
        }
      


        // GET: api/games/search?query=minecraft
        [HttpGet("search")]
        public async Task<ActionResult<List<GameDTO>>> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(
                    "Query is required.");
            }

            var games = await _db.Games.AsNoTracking()
                .Where(g =>
                    g.Name.Contains(query))
                .ToListAsync();

            if(games.Count==0)
            {
                //return Result<List<GameDTO>>.Failure("Game not found");
                return BadRequest("No games found");
            }

            var gamesList = fetchGamesDTO(games);

            //if (gamesList.Count > 0)
            //{
                return Ok(gamesList);
            //}

            // Otherwise call GameBrain.
            //
            // GameBrainService will:
            // 1. Request GameBrain
            // 2. Convert the response
            //// 3. Save it to SQL Server
            //var importedGames =
            //    await _gameBrain.SearchGamesAsync(
            //        query);

            //return Ok(importedGames);
        }


        // GET: api/games/genres
        [HttpGet("genres")]
        public async Task<ActionResult<IEnumerable<string>>> GetGenre()
        {
            var genres = await _db.Games.AsNoTracking()
                .Select(m => m.Genres)
                .Distinct().ToListAsync();

            return Ok(genres);
        }


        // GET: api/games/platforms
        [HttpGet("platforms")]
        public async Task<ActionResult<IEnumerable<string>>> GetPlatforms()
        {
            var conn = _config["ConnectionStrings:GameDatabase"];
            var platform = await _db.Games.AsNoTracking()
                .Select(m => m.Platforms)
                .ToListAsync();

            string combinedPlatformList = "";
            foreach (var item in platform)
            {
                combinedPlatformList = combinedPlatformList +item + "|";
            }

            string[] platforms= combinedPlatformList.Trim().Split("|", StringSplitOptions.TrimEntries).Distinct().ToArray();

            return Ok(platforms);
        }

    }
}

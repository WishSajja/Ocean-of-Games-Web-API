using GameAPI.Data;
using GameAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GameAPI.Services
{
    public class GameBrainService
    {
        private readonly HttpClient _httpClient;
        private readonly GameDbContext _db;
        private readonly IConfiguration _config;

        public GameBrainService(
            HttpClient httpClient,
            GameDbContext db,
            IConfiguration config)
        {
            _httpClient = httpClient;
            _db = db;
            _config = config;
        }

        public async Task<List<Game>> SearchGamesAsync(string query, int limit = 20)
        {

            // Get API key
            var apiKey = _config["GAMEBRAIN_API_KEY"];
                //Environment.GetEnvironmentVariable(
                //    "GAMEBRAIN_API_KEY");

            //if (string.IsNullOrWhiteSpace(apiKey))
            //{
            //    throw new Exception(
            //        "GAMEBRAIN_API_KEY environment variable is missing.");
            //}

            // Create GameBrain URL
            var url =
                $"games?query={Uri.EscapeDataString(query)}&limit={limit}";

            // Create GET request
            using var request = new HttpRequestMessage(HttpMethod.Get, url);

            // Add API key
            request.Headers.Add(
                "x-api-key",
                apiKey);

            // Send request to GameBrain
            using var response =
                await _httpClient.SendAsync(request);

            // Handle errors
            if (response.StatusCode ==
                HttpStatusCode.Unauthorized)
            {
                throw new Exception(
                    "GameBrain returned 401. Check your API key.");
            }

            if (response.StatusCode ==
                HttpStatusCode.PaymentRequired)
            {
                throw new Exception(
                    "GameBrain returned 402. Your daily API quota has been used.");
            }

            if (response.StatusCode ==
                HttpStatusCode.TooManyRequests)
            {
                throw new Exception(
                    "GameBrain returned 429. Too many requests.");
            }

            response.EnsureSuccessStatusCode();

            // Convert JSON response to string then to gamebrainresponse object
            var json =
    await response.Content.ReadAsStringAsync();

            var result =
                JsonSerializer.Deserialize<GameBrainResponse>(json);

            if (result == null)
            {
                return [];
            }

            // Convert GameBrain gamesinto the Game model
            var games = result.Results
                .Select(ConvertGame)
                .ToList();

            // Save the games to SQL Server
            await SaveGamesAsync(games);

            return games;
        }

        private async Task SaveGamesAsync(List<Game> games)
        {
            foreach (var game in games)
            {
                var existingGame =
                    await _db.Games
                        .FirstOrDefaultAsync(
                            g => g.GameBrainId ==
                                 game.GameBrainId);

                if (existingGame == null)
                {
                    // New game
                    _db.Games.Add(game);
                }
                else
                {
                    // Existing game
                    UpdateGame(
                        existingGame,
                        game);
                }
            }

            // Actually save changes to SQL Server
            await _db.SaveChangesAsync();
        }

        private static void UpdateGame(Game existingGame, Game importedGame)
        {
            existingGame.Name =
                importedGame.Name;

            existingGame.Description =
                importedGame.Description;

            existingGame.ReleaseDate =
                importedGame.ReleaseDate;

            existingGame.Rating =
                importedGame.Rating;

            existingGame.RatingCount =
                importedGame.RatingCount;

            existingGame.ImageUrl =
                importedGame.ImageUrl;

            existingGame.GameBrainUrl =
                importedGame.GameBrainUrl;

            existingGame.Developer =
                importedGame.Developer;

            existingGame.Genres =
                importedGame.Genres;

            existingGame.Platforms =
                importedGame.Platforms;

            existingGame.Screenshots =
                importedGame.Screenshots;

            existingGame.CachedAt =
                DateTime.UtcNow;
        }

        private static Game ConvertGame(GameBrainGame game)
        {

            return new Game
            {
                GameBrainId = game.Id,
                Name = game.Name ?? "Unknown",

                Description = game.ShortDescription,

                Rating = game.Rating?.Mean,

                RatingCount = game.Rating?.Count is double count
                    ? (int)count
                    : null,

                ImageUrl = game.Image,

                GameBrainUrl = game.Link,

                Genres = game.Genre,

                Screenshots = string.Join("|", game.Screenshots),

                CachedAt = DateTime.UtcNow
            };
        }
    }
}

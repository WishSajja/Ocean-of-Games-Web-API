using System.Text.Json.Serialization;

namespace GameAPI.Models
{
    public class GameBrainResponse
    {
        [JsonPropertyName("total_results")]
        public int TotalResults { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        [JsonPropertyName("results")]
        public List<GameBrainGame> Results { get; set; } = [];
    }

    public class GameBrainGame
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("year")]
        public double? Year { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("genre")]
        public string? Genre { get; set; }

        [JsonPropertyName("image")]
        public string? Image { get; set; }

        [JsonPropertyName("link")]
        public string? Link { get; set; }

        [JsonPropertyName("rating")]
        public GameBrainRating? Rating { get; set; }

        [JsonPropertyName("screenshots")]
        public List<string> Screenshots { get; set; } = [];

        [JsonPropertyName("short_description")]
        public string? ShortDescription { get; set; }

        [JsonPropertyName("micro_trailer")]
        public string? MicroTrailer { get; set; }

        [JsonPropertyName("gameplay")]
        public string? Gameplay { get; set; }

        [JsonPropertyName("adult_only")]
        public bool AdultOnly { get; set; }
    }

    public class GameBrainRating
    {
        [JsonPropertyName("mean")]
        public double? Mean { get; set; }

        [JsonPropertyName("count")]
        public double? Count { get; set; }
    }

    public class GameBrainItem
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }
}

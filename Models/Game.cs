namespace GameAPI.Models
{
    public class Game
    {
        public int Id { get; set; }

        public int GameBrainId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public double? Rating { get; set; }

        public int? RatingCount { get; set; }

        public string? ImageUrl { get; set; }

        public string? GameBrainUrl { get; set; }

        public string? Developer { get; set; }

        public string? Genres { get; set; }

        public string? Platforms { get; set; }

        public string? Screenshots { get; set; }

        public DateTime CachedAt { get; set; }
    }
}

using GameAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GameAPI.Data
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(
            DbContextOptions<GameDbContext> options)
            : base(options)
        {
        }

        public DbSet<Game> Games => Set<Game>();

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .HasIndex(g => g.GameBrainId)
                .IsUnique();
        }
    }
}

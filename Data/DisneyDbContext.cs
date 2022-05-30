using DisneyAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DisneyAPI.Data
{
    public class DisneyDbContext : DbContext
    {
        public DisneyDbContext(DbContextOptions<DisneyDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Character> Characters { get; set; }
        public DbSet<MovieOrSerie> MoviesOrSeries { get; set; }
        public DbSet<Genre> Genres { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;

namespace MoviesAPIWebApp.Models
{
    public class MoviesAPIContext : DbContext
    {
        public virtual DbSet<Director> Directors { get; set; }
        public virtual DbSet<DirectorMovie> DirectorMovies { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public MoviesAPIContext(DbContextOptions<MoviesAPIContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

    }
}

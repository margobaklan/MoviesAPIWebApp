using Microsoft.AspNetCore.Mvc.Formatters;

namespace MoviesAPIWebApp.Models
{
    public class Movie
    {
        public Movie() 
        {
            DirectorMovies = new List<DirectorMovie>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public virtual Genre Genre { get; set; }
        public virtual ICollection<DirectorMovie> DirectorMovies { get; set; }

    }
}

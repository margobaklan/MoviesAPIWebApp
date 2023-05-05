using Microsoft.AspNetCore.Mvc.Formatters;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string Description { get; set; }
        public int? GenreId { get; set; }
        public virtual Genre? Genre { get; set; }
        public virtual ICollection<DirectorMovie> DirectorMovies { get; set; }

    }
}

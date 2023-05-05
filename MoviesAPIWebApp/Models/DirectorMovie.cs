using System.ComponentModel.DataAnnotations;

namespace MoviesAPIWebApp.Models
{
    public class DirectorMovie
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int DirectorId { get; set; }
        public virtual Director? Director { get; set; }
        public virtual Movie? Movie { get;set; }
    }
}

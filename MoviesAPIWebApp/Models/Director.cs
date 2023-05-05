namespace MoviesAPIWebApp.Models
{
    public class Director
    {
        public Director() 
        {
            DirectorMovies = new List<DirectorMovie>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<DirectorMovie> DirectorMovies { get; set; }
    }
}

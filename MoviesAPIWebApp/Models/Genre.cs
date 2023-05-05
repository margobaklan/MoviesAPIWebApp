using System.ComponentModel.DataAnnotations;

namespace MoviesAPIWebApp.Models
{
    public class Genre
    {
        public Genre() 
        {
            Movies = new List<Movie>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage ="Поле не повинно бути порожнім")]
        [Display(Name="Категорія")]
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }
    }
}

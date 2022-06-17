using System.ComponentModel.DataAnnotations;
namespace MvcMovie.Models
{
    public class Genre
    {
        [Required]
        public int GenreId { get; set; }
        [Required]
        public string Type { get; set; }
    }
}

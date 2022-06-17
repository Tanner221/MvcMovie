using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MvcMovie.Models
{
    public class Movie
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public int GenreId { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }
        [Required]
        public string Rating { get; set; }

        public string ImagePath { get; set; }

        public Genre Genre { get; set; }
    }
}

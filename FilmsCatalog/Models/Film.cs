using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Models
{
    public class Film
    {
        public int Id { get; set; }
       
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
       
        [RegularExpression(@"^[12][0-9]{3}$",
            ErrorMessage = "Date must be in 1000 - 2999")]
        [Required]
        public int Year { get; set; }
        [Required]
        public string Producer { get; set; }
        public string Poster { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [NotMapped] public bool IsHisPostedFilm { get; set; } = false;
    }
}

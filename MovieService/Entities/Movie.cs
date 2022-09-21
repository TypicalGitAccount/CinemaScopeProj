using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieService.Entities
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        public string ImdbId { get; set; }

        [Required]
        [StringLength(220)]
        public string Title { get; set; }

        public string Poster { get; set; }

        [Required]
        public string Year { get; set; }

        [Required]
        public int TypeId { get; set; }

        public virtual MovieType Type { get; set; }

        public string Cast { get; set; }

        public string Plot { get; set; }

        public string Budget { get; set; }

        public string BoxOffice { get; set; }

        public string RatingIMDb { get; set; }

        public double? SiteUsersRating { get; set; }

        public virtual ICollection<Genre> Genres { get; set; } 

        public virtual ICollection<Country> Countries { get; set; }
    }
}

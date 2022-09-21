using MovieService.Dtos.Validation;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MovieService.Dtos
{
    public class MovieDto
    {
        public int Id { get; set; }

        public string ImdbId { get; set; }

        [StringFieldValidation]
        public string Title { get; set; }

        [PosterLinkValidation]
        public string Poster { get; set; }

        [YearStringFieldValidation]
        public string Year { get; set; }

        public int TypeId { get; set; }

        public SelectList MovieTypes { get; set; }

        [StringFieldValidation]
        public string Cast { get; set; }

        [PlotValidation]
        public string Plot { get; set; }

        [MoneyFieldValidation]
        public string Budget { get; set; }

        [MoneyFieldValidation]
        public string BoxOffice { get; set; }

        public string RatingIMDb { get; set; }

        public double? SiteUsersRating { get; set; }
        
        [SelectBoxValidation]
        public List<int> GenreIds { get; set; } = new List<int>();

        public MultiSelectList GenreList { get; set; }

        [SelectBoxValidation]
        public List<int> CountryIds { get; set; } = new List<int>();

        public virtual MultiSelectList CountriesList { get; set; }
    }
}

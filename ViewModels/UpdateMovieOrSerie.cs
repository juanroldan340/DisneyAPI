using DisneyAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace DisneyAPI.ViewModels
{
    public class UpdateMovieOrSerie : AddMovieOrSerie
    {
        [Required]
        public int MovieOrSerieId { get; set; }

        public new MovieOrSerie ToMovieOrSerieModel()
        {
            return new MovieOrSerie
            {
                MovieOrSerieId = MovieOrSerieId,
                Image = Image,
                Title = Title,
                CreationDate = CreationDate,
                GenreId = GenreId,
                Qualification = Qualification
            };
        }
    }
}

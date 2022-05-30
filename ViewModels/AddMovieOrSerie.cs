using DisneyAPI.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace DisneyAPI.ViewModels
{
    public class AddMovieOrSerie
    {
        [Display(Name = "Imagen")]
        public string Image { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Title { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        [Display(Name = "Género")]
        public int? GenreId { get; set; }

        [MaxLength(1)]
        [Range(0, 5)]
        [Display(Name = "Calificación")]
        public int Qualification { get; set; }

        public MovieOrSerie ToMovieOrSerieModel() 
        {
            return new MovieOrSerie
            {
                MovieOrSerieId = null,
                Image = Image,
                Title = Title,
                CreationDate = CreationDate,
                GenreId = GenreId,
                Qualification = Qualification
            };
        }
    }
}

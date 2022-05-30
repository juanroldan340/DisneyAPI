using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DisneyAPI.Models
{
    public class MovieOrSerie
    {
        [Key]
        public int? MovieOrSerieId { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public int? GenreId { get; set; }

        [MaxLength(1)]
        public int Qualification { get; set; }
    }
}

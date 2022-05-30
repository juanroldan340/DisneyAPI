using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DisneyAPI.Models
{
    public class Genre : GenericModel
    {
        [Key]
        public int? GenreId { get; set; }
        public ICollection<MovieOrSerie> MovieOrSerie { get; set; } = null!;
    }
}

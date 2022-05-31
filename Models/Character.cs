using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DisneyAPI.Models
{
    public class Character : GenericModel
    {
        [Key]
        public int? CharacterId { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }
        public string History { get; set; }

        public List<MovieOrSerie> MovieOrSerie { get; set; } = null!;
    }
}

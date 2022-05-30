using DisneyAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace DisneyAPI.ViewModels
{
    public class UpdateCharacter : AddCharacter
    {
        [Required(ErrorMessage = "El Id es obligatorio.")]
        public int CharacterId { get; set; }


        public Character ToCharacterModel()
        {
            return new Character
            {
                CharacterId = CharacterId,
                Image = Image,
                Name = Name,
                Age = Age,
                Weight = Weight,
                History = History,
            };
        } 
    }
}

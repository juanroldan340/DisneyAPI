using DisneyAPI.Models;
using DisneyAPI.Services;
using DisneyAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DisneyAPI.Controllers
{
    [ApiController]
    [Route("/characters")]
    [Authorize]
    public class CharactersController : Controller
    {
        private readonly ICharactersService _service;
        public CharactersController(ICharactersService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var characters = await _service.Get();
            return Ok(characters);
        }

        [HttpGet("{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCharacter(int Id)
        {
            var character = await _service.GetById(Id);

            if (character == null)
                return NotFound("No se ha encontrado el Personaje especificado.");

            return Ok(character);
        }

        [HttpGet("age")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByAge(int age)
        {
            var character = await _service.GetByAge(age);

            if (character == null)
                return NotFound("No se ha encontrado el Personaje especificado.");

            return Ok(character);
        }

        [HttpGet("name")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByName(string name)
        {
            var character = await _service.GetByName(name);

            if (character == null)
                return NotFound("No se encontró el Personaje.");

            return Ok(character);
        }

        [HttpGet("movies")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByMovie(int movieId)
        {
            if (movieId == 0)
                return BadRequest("Ingrese su película o serie.");

            return Ok(await _service.GetByMovie(movieId));
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCharacter character)
        {
            if (!await _service.Add(character))
                return NotFound("Error al agregar al registro.");

            return Ok("Su Personaje ha sido agreagado correctamente.");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            if (!await _service.Delete(Id))
                return NotFound("No se encuentra el registro.");

            return Ok("Su Personaje ha sido eliminado satisfactoriamente.");
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCharacter character)
        {
            var response = new ResponseData();

            if (character == null)
                return BadRequest("Complete los campos correctamente.");

            if (!await _service.Update(character.ToCharacterModel()))
                return NotFound("No se ha encontrado el Personaje.");

            return StatusCode(201, "Su Personaje ha sido modificado correctamente.");
        }
    }
}

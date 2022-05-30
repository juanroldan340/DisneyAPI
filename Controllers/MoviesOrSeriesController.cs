using DisneyAPI.Services;
using DisneyAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DisneyAPI.Controllers
{
    [ApiController]
    [Route("movies")]
    [Authorize]
    public class MoviesOrSeriesController : Controller
    {
        private readonly IMoviesOrSeriesService _service;
        public MoviesOrSeriesController(IMoviesOrSeriesService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var moviesOrSeries = await _service.Get();
            return Ok(moviesOrSeries);
        }

        [HttpGet("{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMovieOrSerie(int Id)
        {
            var movieOrSerie = await _service.GetById(Id);

            if (movieOrSerie == null)
                return NotFound("No se ha encontrado la Película o Serie por el id especificado.");

            return Ok(movieOrSerie);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddMovieOrSerie movieOrSerie) 
        {
            await _service.Add(movieOrSerie.ToMovieOrSerieModel());
            return Ok();
        }

        [HttpGet("{genre}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByGenre(int genre)
        {
            var moviesOrSeries = await _service.GetByGenre(genre);

            if (moviesOrSeries == null)
                return NotFound("No se ha encontrado Película o Serie por el id especificado.");

            return Ok(moviesOrSeries);
        }

        [HttpGet("{order}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOrdered(string order)
        {
            if (order == string.Empty)
                return BadRequest("El campo debe ser obligatorio.");

            order = order.ToUpper();

            return Ok(await _service.GetOrdered(order));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            if (!await _service.Delete(Id))
                return NotFound("No se encuentra el registro.");

            return Ok("La Serie o Película ha sido eliminada satisfactoriamente.");
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateMovieOrSerie movieOrSerie)
        {
            if (movieOrSerie == null)
                return BadRequest("Complete los campos correctamente.");

            if (!await _service.Update(movieOrSerie.ToMovieOrSerieModel()))
                return NotFound("No se ha encontrado la Película o Serie.");

            return Ok("La Serie o Película ha sido modificada correctamente.");
        }
    }
}

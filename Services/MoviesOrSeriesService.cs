using DisneyAPI.Models;
using DisneyAPI.Repositories;
using DisneyAPI.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DisneyAPI.Services
{
    public interface IMoviesOrSeriesService : IGenericService<MovieOrSerie>
    {
        Task<List<MovieOrSerie>> GetOrdered(string order);
        Task<List<MovieOrSerie>> GetByGenre(int genreId);
        Task<List<MovieOrSerie>> GetByTitle(string title);
        Task<List<GetMoviesOrSeries>> Get();
    }

    public class MoviesOrSeriesService : GenericService<MovieOrSerie>, IMoviesOrSeriesService
    {
        private readonly IMoviesOrSeriesRepository _repository;

        public MoviesOrSeriesService(IMoviesOrSeriesRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<List<GetMoviesOrSeries>> Get() 
        {
            var moviesOrSeries = await _repository.Get();

            var model = new List<GetMoviesOrSeries>();

            foreach (var m in moviesOrSeries) 
            {
                model.Add(new GetMoviesOrSeries
                {
                    Image = m.Image,
                    Title = m.Title,
                    CreationDate = m.CreationDate
                });
            }

            return model;
        }

        public async Task<List<MovieOrSerie>> GetOrdered(string order) => await _repository.GetOrdered(order);

        public async Task<List<MovieOrSerie>> GetByGenre(int genreId) => await _repository.GetByGenre(genreId);

        public async Task<List<MovieOrSerie>> GetByTitle(string title) => await _repository.GetByTitle(title);
    }
}

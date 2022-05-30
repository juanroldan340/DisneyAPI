using DisneyAPI.Models;
using DisneyAPI.Repositories;
using DisneyAPI.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DisneyAPI.Services
{
    public interface ICharactersService : IGenericService<Character> 
    {
        Task<List<Character>> GetByName(string name);
        Task<List<Character>> GetByAge(int age);
        Task<List<Character>> GetByMovie(int movieId);
        Task<List<GetCharacters>> Get();
        Task<bool> Add(AddCharacter character);
    }

    public class CharactersService : GenericService<Character>, ICharactersService
    {
        private readonly ICharactersRepository _repository;
        private readonly IMoviesOrSeriesRepository _moviesOrSeriesRepository;
        public CharactersService(ICharactersRepository repository, IMoviesOrSeriesRepository moviesOrSeriesRepository) : base(repository)
        {
            _repository = repository;
            _moviesOrSeriesRepository = moviesOrSeriesRepository;
        }

        public async Task<List<GetCharacters>> Get() 
        {
            var characters = await _repository.Get();
            var model = new List<GetCharacters>();

            foreach (var c in characters) {
                model.Add(new GetCharacters
                {
                    Image = c.Image,
                    Name = c.Name
                });
            }

            return model;
        }

        public async Task<bool> Add(AddCharacter character)
        {
            var movieOrSerie = character != null ? await _moviesOrSeriesRepository.GetById((int)character.MovieOrSerieId) : null; 

            var characterList = new Character
            {
                CharacterId = null,
                Name = character.Name,
                Image = character.Image,
                Age = character.Age,
                Weight = character.Weight,
                History = character.History,
                MovieOrSerie = movieOrSerie as ICollection<MovieOrSerie>
            };

            if (!await _repository.Add(characterList))
                return false;

            return true;
        }

        public async Task<List<Character>> GetByAge(int age) => await _repository.GetByAge(age);

        public async Task<List<Character>> GetByMovie(int movieId) => await _repository.GetByMovie(movieId);
        
        public async Task<List<Character>> GetByName(string name) => await _repository.GetByName(name);
    }
}


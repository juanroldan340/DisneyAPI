using DisneyAPI.Data;
using DisneyAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.Repositories
{
    public interface ICharactersRepository : IGenericRepository <Character>
    {
        Task<List<Character>> GetByName(string name);
        Task<List<Character>> GetByAge(int age);
        Task<List<Character>> GetByMovie(int movieId);
    }
    public class CharactersRepository : GenericRepository<Character>, ICharactersRepository
    {
        private readonly DisneyDbContext _context;
        public CharactersRepository(DisneyDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Character>> GetByAge(int age)
        {
            try
            {
                var characters = await _context.Characters.Where(c => c.Age == age).ToListAsync();
                return characters;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Character>> GetByMovie(int movieId)
        {
            try
            {
                var characters = await _context.Characters.Include(m => m.MovieOrSerie.Select(m => m.MovieOrSerieId == movieId)).ToListAsync();
                return characters;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Character>> GetByName(string name)
        {
            try
            {
                var characters = await _context.Characters.Where(c => c.Name == name).ToListAsync();
                return characters;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

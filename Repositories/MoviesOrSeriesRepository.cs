using DisneyAPI.Data;
using DisneyAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.Repositories
{
    public interface IMoviesOrSeriesRepository : IGenericRepository<MovieOrSerie>
    {
        Task<List<MovieOrSerie>> GetOrdered(string order);
        Task<List<MovieOrSerie>> GetByGenre(int genreId);
        Task<List<MovieOrSerie>> GetByTitle(string title);
    }
    public class MoviesOrSeriesRepository : GenericRepository<MovieOrSerie>, IMoviesOrSeriesRepository
    {
        private readonly DisneyDbContext _context;
        public MoviesOrSeriesRepository(DisneyDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<MovieOrSerie>> GetByGenre(int genreId)
        {
            try
            {
                var moviesOrSeries = await _context.MoviesOrSeries.Where(m => m.GenreId.Equals(genreId)).ToListAsync();
                return moviesOrSeries;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<MovieOrSerie>> GetByTitle(string title)
        {
            try
            {
                var moviesOrSeries = await _context.MoviesOrSeries.Where(m => m.Title.Contains(title)).ToListAsync();
                return moviesOrSeries;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<MovieOrSerie>> GetOrdered(string order)
        {
            try
            {
                var moviesOrSeries = new List<MovieOrSerie>();
                if (order == "ASC")
                    moviesOrSeries = await (from c in _context.MoviesOrSeries
                                    orderby c.Title ascending
                                    select c).ToListAsync();
                if (order == "DESC")
                    moviesOrSeries = await (from c in _context.MoviesOrSeries
                                    orderby c.Title descending
                                    select c).ToListAsync();

                return moviesOrSeries;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

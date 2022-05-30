using DisneyAPI.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DisneyAPI.Repositories
{
    public interface IGenericRepository<TEntity>
    {
        Task<bool> Add(TEntity entity);
        Task<bool> Delete(int Id);
        Task<bool> Update(TEntity entity);
        Task<List<TEntity>> Get();
        Task<TEntity> GetById(int Id);
    }
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DisneyDbContext _context;

        public GenericRepository(DisneyDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Delete(int Id)
        {
            _context.Set<TEntity>().Remove(await GetById(Id));

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<TEntity>> Get()
        {
            var entities = await _context.Set<TEntity>().ToListAsync();
            return entities;
        }

        public async Task<TEntity> GetById(int Id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(Id);
            return entity;
        }

        public async Task<bool> Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

}

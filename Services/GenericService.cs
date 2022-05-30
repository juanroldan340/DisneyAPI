using DisneyAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DisneyAPI.Services
{
    public interface IGenericService <TEntity> where TEntity : class
    {
        Task<bool> Add(TEntity entity);
        Task<bool> Delete(int Id);
        Task<bool> Update(TEntity entity);
        Task<TEntity> GetById(int Id);
    }
    public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _repository; 
        public GenericService(IGenericRepository<TEntity> repository)
        {
            _repository = repository;
        }
        public async Task<bool> Add(TEntity entity)
        {
            if (entity == null)
                throw new Exception("Los valores enviados están vacíos");

            if (!await _repository.Add(entity))
                return false;
            
            return true;
        }

        public async Task<bool> Delete(int Id)
        { 
            if(!await _repository.Delete(Id))
                return false;

            return true;
        }

        public async Task<TEntity> GetById(int Id)
        {
            var entity = await _repository.GetById(Id);
            return entity;
        }

        public async Task<bool> Update(TEntity entity)
        {
            if(!await _repository.Update(entity))
                return false;

            return true;
        }
    }
}

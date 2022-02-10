using Application.Common.Interfaces;
using Domain.Common;
using Infastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infastructure.Persistence.Repositories
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly ApplicationContext _appContext;
        protected readonly ILoggerManager _logger;

        public GenericRepository(ApplicationContext applicationContext, ILoggerManager logger)
        {
            _appContext = applicationContext;
            _logger = logger;
        }

        public virtual async Task CreateAsync(TEntity entity) =>
            await _appContext.Set<TEntity>().AddAsync(entity);

        public virtual async Task DeleteAsync(Guid id)
        {
            var existing = await _appContext.Set<TEntity>().FindAsync(id);

            if (existing == null)
            {
                _logger.LogError($"An entity with id: {id} doesn't exist in the database");
                return;
                //return NotFound
            }

            _appContext.Set<TEntity>().Remove(existing);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync() =>
            await _appContext.Set<TEntity>().ToListAsync();

        public virtual async Task<TEntity> GetByIdAsync(Guid id) =>
            await _appContext.Set<TEntity>().FindAsync(id);

        public virtual async Task UpdateAsync(TEntity entity)
        {
            var isExists = await _appContext.Set<TEntity>().AnyAsync(x => x.Id.Equals(entity.Id));

            if (!isExists)
            {
                _logger.LogError($"The entity of type {typeof(TEntity)} with id: {entity.Id} doent's exist in the database");
                return;
                //return NotFound();
            }

            _appContext.Set<TEntity>().Update(entity);
        }
    }
}
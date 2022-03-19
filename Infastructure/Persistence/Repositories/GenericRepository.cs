using Application.Common.Exceptions;
using Application.Common.Interfaces.Contexts;
using Application.Common.Interfaces.Managers;
using Application.Common.Interfaces.Repositories;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infastructure.Persistence.Repositories
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly IApplicationDbContext AppContext;
        protected readonly ILoggerManager Logger;

        public GenericRepository(IApplicationDbContext applicationContext, ILoggerManager logger)
        {
            AppContext = applicationContext;
            Logger = logger;
        }

        public virtual async Task<List<TEntity>> GetAllAsync() =>
            await AppContext.Set<TEntity>()
            .ToListAsync();

        public virtual async Task<TEntity> GetByIdAsync(Guid id) =>
            await AppContext.Set<TEntity>()
            .Where(entity => entity.Id == id)
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"An entity of {typeof(TEntity)} type with id: {id} not found");

        public virtual async Task<TEntity> GetByIdReadOnlyAsync(Guid id) =>
            await AppContext.Set<TEntity>()
            .Where(p => p.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync() ?? throw new NotFoundException($"An entity of {typeof(TEntity)} type with id: {id} not found");

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(TEntity));
            }

            await AppContext.Set<TEntity>().AddAsync(entity);
            await AppContext.SaveChangesAsync();

            return entity;
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var existing = await AppContext.Set<TEntity>().FindAsync(id);
            if (existing == null)
            {
                throw new NotFoundException($"A fridge with id: {id} doesn't exist in the database");
            }

            AppContext.Set<TEntity>().Remove(existing);
            await AppContext.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            bool isExists = await AppContext.Set<TEntity>().AnyAsync(e => e.Id == entity.Id);
            if (!isExists)
            {
                throw new NotFoundException($"A fridge with id:{entity.Id} doesn't exist in the database");
            }

            AppContext.Set<TEntity>().Update(entity);
            await AppContext.SaveChangesAsync();
        }
    }
}
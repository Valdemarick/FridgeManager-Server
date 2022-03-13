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
        protected readonly IApplicationDbContext appContext;
        protected readonly ILoggerManager logger;

        public GenericRepository(IApplicationDbContext applicationContext, ILoggerManager logger)
        {
            appContext = applicationContext;
            this.logger = logger;
        }

        public virtual async Task<List<TEntity>> GetAllAsync() =>
            await appContext.Set<TEntity>()
            .ToListAsync();

        public virtual async Task<TEntity> GetByIdAsync(Guid id) =>
            await appContext.Set<TEntity>()
            .Where(entity => entity.Id == id)
            .FirstOrDefaultAsync();

        public virtual async Task<TEntity> GetByIdReadOnlyAsync(Guid id) =>
            await appContext.Set<TEntity>()
            .Where(p => p.Id.Equals(id))
            .AsNoTracking()
            .FirstOrDefaultAsync();

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(TEntity));
            }

            await appContext.Set<TEntity>().AddAsync(entity);
            await appContext.SaveChangesAsync();

            return entity;
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var existing = await appContext.Set<TEntity>().FindAsync(id);
            if (existing == null)
            {
                throw new ArgumentException($"A fridge with id: {id} doesn't exist in the database");
            }

            appContext.Set<TEntity>().Remove(existing);
            await appContext.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            var isExists = await appContext.Set<TEntity>().AnyAsync(e => e.Id.Equals(entity.Id));
            if (!isExists)
            {
                throw new ArgumentException($"A fridge with id:{entity.Id} doesn't exist in the database");
            }

            appContext.Set<TEntity>().Update(entity);
            await appContext.SaveChangesAsync();
        }
    }
}
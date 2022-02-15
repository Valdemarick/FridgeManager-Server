using Application.Common.Interfaces;
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

        public virtual async Task CreateAsync(TEntity entity) =>
            await appContext.Set<TEntity>()
            .AddAsync(entity);

        public virtual async Task DeleteAsync(Guid id)
        {
            var existing = await appContext.Set<TEntity>().FindAsync(id);
            if (existing == null)
            {
                logger.LogError($"An entity with id: {id} doesn't exist in the database");
                return;
            }

            appContext.Set<TEntity>().Remove(existing);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync() =>
            await appContext.Set<TEntity>().ToListAsync();

        public virtual async Task<TEntity> GetByIdAsync(Guid id) =>
            await appContext.Set<TEntity>()
            .FindAsync(id);

        public virtual async Task<TEntity> GetByIdReadOnlyAsync(Guid id) =>
            await appContext.Set<TEntity>()
            .Where(p => p.Id.Equals(id))
            .AsNoTracking()
            .SingleOrDefaultAsync();

        public virtual async Task UpdateAsync(TEntity entity)
        {
            var existing = await appContext.Set<TEntity>().FindAsync(entity.Id);

            if (existing == null)
            {
                logger.LogError($"The entity of type {typeof(TEntity)} with id: {entity.Id} doent's exist in the database");
                return;
            }

            appContext.Set<TEntity>().Update(entity);
        }
    }
}
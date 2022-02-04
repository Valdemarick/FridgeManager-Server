using Application.Common.Interfaces;
using Infastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Infastructure.Persistence.Repositories
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected ApplicationContext _appContext;

        public GenericRepository(ApplicationContext applicationContext) => _appContext = applicationContext;

        public IQueryable<TEntity> FindAll(bool tracking)
        {
            if (tracking)
                return _appContext.Set<TEntity>();
            else
                return _appContext.Set<TEntity>().AsNoTracking();
        }

        public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool tracking)
        {
            if (tracking)
                return _appContext.Set<TEntity>().Where(expression);
            else
                return _appContext.Set<TEntity>().Where(expression).AsNoTracking();
        }

        public void Create(TEntity entity) => _appContext.Set<TEntity>().Add(entity);

        public void Delete(TEntity entity) => _appContext.Set<TEntity>().Remove(entity);

        public void Update(TEntity entity) => _appContext.Set<TEntity>().Update(entity);
    }
}
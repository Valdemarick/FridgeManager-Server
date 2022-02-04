using System;
using System.Linq;
using System.Linq.Expressions;

namespace Application.Common.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> FindAll(bool tracking);
        IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression,
                                      bool tracking);
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}

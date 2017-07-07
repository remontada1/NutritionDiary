using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Infrastructure
{
    interface IRepository<TEntity>  where TEntity  : class
    {
        IEnumerable<TEntity> IncludeAll(params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TEntity> GetAll();
        
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> FindBy( Expression <Func<TEntity,bool>> predicate);

        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
    }
}

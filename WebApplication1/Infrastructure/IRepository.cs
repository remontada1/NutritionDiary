using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Infrastructure
{
     public interface IRepository<TEntity>  where TEntity  : class
    {
        
        IEnumerable<TEntity> GetAll();
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        TEntity GetById(int id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(int id);
        void RemoveByEntity(TEntity entity);
        TEntity FindById(object id);
    }
}

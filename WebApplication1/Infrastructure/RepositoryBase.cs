using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplication1.DAL;

namespace WebApplication1.Infrastructure
{
    public class IRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private CustomerContext dataContext;
        private readonly IDbSet<TEntity> dbSet;
        protected IDbFactory DbFactory
        {
            get; private set;
        }

        protected CustomerContext DbContext
        {
            get {  return dataContext ?? ( dataContext = DbFactory.Init()); }
        }

       protected  IRepository(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            dbSet = DbContext.Set<TEntity>();

        }



       public IEnumerable<TEntity> IncludeAll(params System.Linq.Expressions.Expression<Func<TEntity, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

       public IEnumerable<TEntity> GetAll(Func<TEntity, bool> predicate = null)
        {
            throw new NotImplementedException();
        }

       public TEntity Get(Func<TEntity, bool> predicate)
        {
            throw new NotImplementedException();
        }

       public IEnumerable<TEntity> FindBy(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApplication1.DAL;

namespace WebApplication1.Infrastructure
{
    public  abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
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

       protected  RepositoryBase(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            dbSet = DbContext.Set<TEntity>();

        }


       public IEnumerable<TEntity> GetAll()
       {
           return dbSet.ToList();
       }
        

       

       public  TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.Where(predicate).FirstOrDefault<TEntity>();
        }

       public virtual TEntity GetById(int id)
       {
           return dbSet.Find(id);
              
       }

     /*  public IEnumerable<TEntity> FindBy(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.Where(predicate).ToList();
        } */

        public  virtual void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public  virtual void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        public  virtual void Remove(TEntity entity)
        {
            dbSet.Remove(entity);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.DAL;
using WebApplication1.Infrastructure;

namespace WebApplication1.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
         CustomerContext dbContext;

        public CustomerContext Init()
        {
            return dbContext ?? (dbContext = new CustomerContext());
        }

        protected override  void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
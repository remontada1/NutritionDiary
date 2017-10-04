using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication1.DAL;
using WebApplication1.Repository;

namespace WebApplication1.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory dbFactory;
        private CustomerContext dbContext;
        private IUserRepository _userRepository;
        private IExternalLoginRepository _externalLoginRepository;
        public UnitOfWork(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public CustomerContext DbContext
        {
            get { return dbContext ?? (dbContext = dbFactory.Init()); }
        }

        public IUserRepository UserRepository
        {
            get { return _userRepository ?? (_userRepository = new UserRepository(dbFactory)); }
        }

        public IExternalLoginRepository ExternalLoginRepository
        {
            get { return _externalLoginRepository ?? (_externalLoginRepository = new ExternalLoginRepository(dbFactory)); }
        }
        public void Commit()
        {
             DbContext.Commit();
        }

        public Task<int> CommitAsync()
        {
           return DbContext.CommitAsync();
        }


    }
}
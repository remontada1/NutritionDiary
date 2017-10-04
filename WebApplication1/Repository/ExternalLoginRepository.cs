using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using WebApplication1.Infrastructure;
using WebApplication1.DAL;
using System.Data.Entity;
using System.Web.Http;
using System.Threading.Tasks;


namespace WebApplication1.Repository
{
    public class ExternalLoginRepository : RepositoryBase<ExternalLogin>, IExternalLoginRepository
    {
        public ExternalLoginRepository(IDbFactory dbFactory)
            : base(dbFactory) { }


        public ExternalLogin GetByProviderAndKey(string loginProvider, string providerKey)
        {
            return DbContext.Logins.FirstOrDefault(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey);
        }

        public Task<ExternalLogin> GetByProviderAndKeyAsync(string loginProvider, string providerKey)
        {
            return DbContext.Logins.FirstOrDefaultAsync(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey);
        }

    }

    public interface IExternalLoginRepository : IRepository<ExternalLogin>
    {
        ExternalLogin GetByProviderAndKey(string loginProvider, string providerKey);
        Task<ExternalLogin> GetByProviderAndKeyAsync(string loginProvider, string ProviderKey);
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WebApplication1.Infrastructure;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {

        public RoleRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }


        public Role FindByName(string roleName)
        {
            return DbContext.Roles.FirstOrDefault(r => r.Name == roleName);
        }

        public Task<Role> FindByNameAsync(string roleName)
        {
            return DbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        }

        public Task<Role> FindByNameAsync(CancellationToken cancellationToken, string roleName)
        {
            return DbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken);
        }
    }

    public interface IRoleRepository : IRepository<Role>
    {
        Role FindByName(string roleName);
        Task<Role> FindByNameAsync(string roleName);
        Task<Role> FindByNameAsync(CancellationToken cancellationToken, string roleName);
    }
}




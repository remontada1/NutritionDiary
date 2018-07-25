using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Repository;

namespace WebApplication1.Infrastructure
{
    public interface IUnitOfWork
    {
        IRoleRepository RoleRepository { get; }
        IUserRepository UserRepository { get; }

        IExternalLoginRepository ExternalLoginRepository { get; }
        void Commit();
        Task<int> CommitAsync();
    }
}

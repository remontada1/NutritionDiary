using System.Linq;
using System.Data.Entity;
using WebApplication1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApplication1.Infrastructure;
using System.Threading.Tasks;


namespace WebApplication1.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private UserManager<IdentityUser> _userManager { get;  set; }
        public UserRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
            
        }

        public User FindByUsername(string username)
        {
            return DbContext.Users.FirstOrDefault(x => x.UserName == username);
        }
        public Task<User> FindByUsernameAsync(string username)
        {
            return  DbContext.Users.FirstOrDefaultAsync(x => x.UserName == username);
        }    
    }


    public interface IUserRepository : IRepository<User>
    {
        User FindByUsername(string username);
        Task<User> FindByUsernameAsync(string username);
       
    }

}
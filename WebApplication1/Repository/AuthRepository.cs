using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using WebApplication1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using WebApplication1.Models;
using WebApplication1.DAL;
using WebApplication1.Infrastructure;
using System.Threading.Tasks;


namespace WebApplication1.Repository
{
    public class AuthRepository : RepositoryBase<Food>
    {
        public AuthRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        private UserManager<IdentityUser> _userManager;

        public async Task<IdentityResult> RegisterUser(UserBindingModel userModel)
        {
            IdentityUser identityUser = new IdentityUser
            {
                UserName = userModel.Email
            };

            var result = await _userManager.CreateAsync(identityUser, userModel.Password);

            return result;
        }



    }
}
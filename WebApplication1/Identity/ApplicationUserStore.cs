using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.DAL;
using WebApplication1.Models;

namespace WebApplication1.Identity
{
    public class ApplicationUserStore : UserStore<ApplicationUser>, IUserStore
    {
        public ApplicationUserStore(CustomerContext context) : base(context) { }
    }


    public interface IUserStore : IUserStore<ApplicationUser> { }

}
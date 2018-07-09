using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using WebApplication1.Infrastructure;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace WebApplication1.Repository
{
    public class GuidRepository : RepositoryBase<User>, IGuidRepository
    {
        public GuidRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }
        public User GetUserByGuid()
        {
            Guid guid = Guid.Empty;
            var currentUserId = HttpContext.Current.User.Identity.GetUserId();

            guid = new Guid(currentUserId); //convert to guid format

            var user = DbContext.Users.Find(guid);

            return user;
        }
    }
    public interface IGuidRepository
    {
        User GetUserByGuid();
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Role
    {
        private ICollection<User> _users;

        public Guid RoleId { get; set; }

        public string Name { get; set; }

        public ICollection<User> Users
        {
            get { return _users ?? (_users = new List<User>()); }
            set { _users = value; }
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class ExternalLogin
    {
        private User _user;

        public virtual string LoginProvider { get; set; }
        public virtual string ProviderKey { get; set; }
        public virtual Guid UserId { get; set; }

        public virtual User User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
                UserId = value.Id;
            }
        }

    }
}
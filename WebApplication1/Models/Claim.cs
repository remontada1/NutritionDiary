﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Claim
    {

        private User _user;

        public virtual int ClaimId { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual string ClaimType { get; set; }
        public virtual string ClaimValue { get; set; }

        public virtual User User
        {
            get { return _user; }
            set {
                if (value == null)
                    throw new ArgumentNullException("value");
                _user = value;
                UserId = value.Id;

            }
        }
    }
}
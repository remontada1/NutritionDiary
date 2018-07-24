using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;

using Owin;

namespace WebApplication1.Models
{
    public class ApplicationUser : IdentityUser
    {


        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        public DateTime JoinDate { get; set; }

        public ICollection<Meal> Meals { get; set; }

        public ApplicationUser()
        {
            Meals = new List<Meal>();
        }


    }

}
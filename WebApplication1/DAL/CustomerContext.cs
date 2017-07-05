using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace WebApplication1.DAL
{
    public class CustomerContext : IdentityDbContext<Customer>
    {
       public CustomerContext()
           : base("name=Schedule")  { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Food> Foods {get; set;}
        public DbSet<Meal> Meals { get; set; }
        public DbSet<CustomerData> CustomersData { get; set; }
        public DbSet<MealType> MealTypes { get; set; }

        public virtual void Commit()
        {
            base.SaveChanges();
        }
    }
}
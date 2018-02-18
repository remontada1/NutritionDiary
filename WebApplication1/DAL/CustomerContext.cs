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
using System.Threading.Tasks;
using WebApplication1.Configuration;

namespace WebApplication1.DAL
{
    public class CustomerContext : DbContext
    {
        public CustomerContext()
            : base("Schedule")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
               
        public DbSet<Food> Foods { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<CustomerData> CustomersData { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ExternalLogin> Logins { get; set; }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        public Task<int> CommitAsync()
        {
             return base.SaveChangesAsync();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new ExternalLoginConfiguration());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using System.Data.Entity;



namespace WebApplication1.DAL
{
    public class CustomerContext : DbContext
    {
       public CustomerContext()
           : base("Schedule")  { }
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
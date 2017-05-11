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
            : base("name=Schedule")
        {

        }
        public DbSet<Customer> Customers { get; set; }



    }
}
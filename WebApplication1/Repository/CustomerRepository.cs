using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplication1.Infrastructure;
using WebApplication1.Models;
using WebApplication1.DAL;



namespace WebApplication1.Infrastructure
{

    [Authorize]
    public class CustomerRepository : IRepository<Customer>
    {
        private CustomerContext context = new CustomerContext();


        public IEnumerable<Customer> GetAll()
        {
           return context.Customers; 
         }
        public Customer Get(int Id)
        {
            return context.Customers.Single(x => x.Id == Id);
        }


        [HttpPost]
        public  void  Create(Customer item)
        {
            if(item == null)
            {
                throw new ArgumentNullException("item");
            }
           context.Customers.Add(item);
           context.SaveChanges();
                 
        }

        public void Remove(int Id)
        {
            Customer customer = context.Customers.Find(Id);
            context.Customers.Remove(customer);
            context.SaveChanges();  
        }

        public void  Update(Customer item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            Customer updateCustomer = context.Customers.FirstOrDefault(c => c.Id == item.Id);


            context.SaveChanges();
            

        }
    }
}

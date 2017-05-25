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
using System.Web.Http.HttpGetAttribute;
using System.Web.Http.ModelBinding;
using WebApplication1.DAL;
using WebApplication1.Models;


namespace WebApplication1.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private CustomerContext context = new CustomerContext();

       

        public IQueryable<Customer> GetAll()
        {
           return context.Customers; 
         }
        public Customer GetById(int Id)
        {
            return context.Customers.Single(x => x.Id == Id);
        }


        [HttpPost]
        public  Customer Add(Customer item)
        {
            if(item == null)
            {
                throw new ArgumentNullException("item");
            }
           context.Customers.Add(item);
           context.SaveChanges();
           return item;       
        }

        public void Remove(int Id)
        {
            Customer customer = context.Customers.Find(Id);
            context.Customers.Remove(customer);
            context.SaveChanges();  
        }

        public Customer Update(Customer item)
        {
            if(item ==null)
            {
                throw new ArgumentNullException("item");
            }
            Customer updateCustomer = context.Customers.FirstOrDefault(c => c.Id == item.Id);
            updateCustomer.FirstName = item.FirstName;
            updateCustomer.LastName = item.LastName;
            updateCustomer.Height = item.Height;
            updateCustomer.Weight = item.Weight;
             
            context.SaveChanges();
            return item;
            
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Description;
using System.Web;
using WebApplication1.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure; 
using WebApplication1.DAL;

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

        public  Customer Add(Customer item)
        {
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
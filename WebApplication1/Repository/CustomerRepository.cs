using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using WebApplication1.DAL;

namespace WebApplication1.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        public CustomerContext context;

        public CustomerRepository(CustomerContext context)
        {
            this.context = context;
        }

        public IEnumerable<Customer> GetAll()
        {
            return context.Customers;
        }
        public Customer GetById(int Id)
        {
            
        }

        public Customer Add(Customer item)
        {
            
        }

        public void Remove(int Id)
        {
            
        }
    }
}
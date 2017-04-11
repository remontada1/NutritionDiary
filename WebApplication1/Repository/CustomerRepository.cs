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
        private CustomerContext context = new CustomerContext();

        public IEnumerable<Customer> Customers
        {
            get { return context.Customers; }
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
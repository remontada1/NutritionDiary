using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        CustomerDbEntities CustomerEntities = new CustomerDbEntities();

        public IEnumerable<Customer> GetAll()
        {
            
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
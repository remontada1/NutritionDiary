using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;  
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CustomerController : ApiController
    {
        ICustomerRepository repository;

        public CustomerController(ICustomerRepository repository)
        {
            this.repository = repository;
        }


        public IEnumerable<Customer> GetAll()
        {
            return repository.GetAll();
        }

        public Customer GetById(int id)
        {
            return repository.GetById(id);
        }

        public Customer Add(Customer item)
        {
            item = repository.Add(item);
            return item;
        }

        public void Remove(int id)
        {
            Customer customer = repository.GetById(id);
            repository.Remove(id);
        }
        
    }
}
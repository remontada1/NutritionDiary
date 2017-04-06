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
        ICustomerRepository _repository;

        public CustomerController(ICustomerRepository repository)
        {
            _repository = repository;
        }


        List<Customer> customers = new List<Customer>()
        {
            new Customer { Id = 1, Name = "Иван", OrderId = 4, PhoneNumber = "+380634750253"},
            new Customer { Id = 2, Name = "Den", OrderId = 3, PhoneNumber= "+380637350223"}
        };



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
            item = _repository.Add(item);
            return item;
        }

        public void Remove(int id)
        {
            Customer customer = _repository.GetById(id);
            _repository.Remove(id);
        }
        
    }
}
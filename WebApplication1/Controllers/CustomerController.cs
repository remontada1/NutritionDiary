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
        List<Customer> customers = new List<Customer>()
        {
            new Customer { Id = 1, Name = "Иван", OrderId = 4, PhoneNumber = "+380634750253"},
            new Customer { Id = 2, Name = "Den", OrderId = 3, PhoneNumber= "+380637350223"}
        };
    }
}
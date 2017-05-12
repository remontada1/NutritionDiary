using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Models
{
    public interface ICustomerRepository
    {
        IQueryable<Customer> GetAll();
        Customer GetById(int Id);
        Customer Add(Customer item);
        void Remove(int Id);

        Customer Update(Customer item);

    }
}

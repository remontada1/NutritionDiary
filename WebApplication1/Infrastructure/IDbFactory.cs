using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.DAL;

namespace WebApplication1.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        CustomerContext Init();
    }
}
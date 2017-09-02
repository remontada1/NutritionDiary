using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using WebApplication1.Repository;
using WebApplication1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using WebApplication1.DAL;
using WebApplication1.Infrastructure;
using System.Threading.Tasks;

namespace WebApplication1.Service
{

     interface IAuthService
    {
         Task<IdentityResult> RegisterUser(UserBindingModel userModel);
    }
    public class AuthService : IAuthService
    {

        IAuthRepository authRepository;
        IUnitOfWork unitOfWork;

        AuthService( IAuthRepository authRepository, IUnitOfWork unitOfWork)
        {
            this.authRepository = authRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<IdentityResult> RegisterUser(UserBindingModel userModel)
        {
            return await authRepository.RegisterUser(userModel);
        }
    }
}
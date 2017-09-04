using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using WebApplication1.Models;
using WebApplication1.Infrastructure;
using AttributeRouting.Web.Http;
using WebApplication1.Service;
using WebApplication1.Mappings;
using WebApplication1.ViewModels;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using WebApplication1.DAL;


namespace WebApplication1.Controllers
{
    public class AuthController : ApiController
    {
        private readonly IAuthService authService;

        AuthController(IAuthService authService)
        {
            this.authService = authService;
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(UserBindingModel userModel)
        {
            IdentityResult result = await authService.RegisterUser(userModel);

            return Content(HttpStatusCode.OK, "User created");
        }
    }
}

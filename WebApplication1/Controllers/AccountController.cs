using System;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApplication1.Identity;
using WebApplication1.Models;
using System.Threading.Tasks;
using System.Net;

namespace WebApplication1.Controllers
{


    public class AccountController : ApiController
    {
        private readonly UserManager<Identity.ApplicationUser, Guid> _userManager;
        public AccountController(UserManager<Identity.ApplicationUser, Guid> userManager)
        {
            _userManager = userManager;
        }

        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(UserBindingModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.NotAcceptable, "Not valid");
            }
            var user = new ApplicationUser() { UserName = userModel.UserName };
            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (result.Succeeded)
            {
                return Content(HttpStatusCode.OK, " succeed");
            }
            else
            {
                return Content(HttpStatusCode.NotModified, "Not succeed");
            }
                

        }
        private Guid getGuid(string value)
        {
            var result = default(Guid);
            Guid.TryParse(value, out result);
            return result;
        }
    }
}

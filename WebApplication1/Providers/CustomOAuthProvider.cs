using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using WebApplication1.DAL;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApplication1.Identity;
using System.Security.Claims;
using Microsoft.Owin.Security;
using System.Web.Http.Cors;

namespace WebApplication1.Providers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {

        private readonly UserManager<ApplicationUser, Guid> _userManager;

        public CustomOAuthProvider(UserManager<ApplicationUser, Guid> userManager)
        {
            _userManager = userManager;
                
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }
        
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = "*";
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
            ApplicationUser user = await _userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The username or password is incorrect");
                return;
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(_userManager, "JWT");

            var ticket = new AuthenticationTicket(oAuthIdentity, null);

            context.Validated(ticket);
        }
    }
}
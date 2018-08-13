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
using WebApplication1.Models;

namespace WebApplication1.Providers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {

        private readonly UserManager<ApplicationUser, Guid> _userManager;
        private readonly RoleManager<Identity.IdentityRole, Guid> _roleManager;

        public CustomOAuthProvider(UserManager<ApplicationUser, Guid> userManager, RoleManager<Identity.IdentityRole, Guid> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;

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

            var roles = await _userManager.GetRolesAsync(user.Id);
            
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            identity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.Name, context.UserName));

            identity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.Role, "User"));
            identity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(_userManager, OAuthDefaults.AuthenticationType);

            var ticket = new AuthenticationTicket(identity, null);

            context.Validated(ticket);
        }
    }
}
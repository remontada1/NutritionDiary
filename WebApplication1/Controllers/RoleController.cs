using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication1.Identity;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class RoleController : ApiController
    {
        private readonly UserManager<ApplicationUser, Guid> _userManager;
        private readonly RoleManager<IdentityRole, Guid> _roleManager;

        public RoleController(UserManager<ApplicationUser, Guid> userManager, RoleManager<IdentityRole, Guid> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IHttpActionResult> GetAllRoles()
        {
            var currentUserGuidId = User.Identity.GetUserId();
            var currentGuidId = new Guid(currentUserGuidId);

            var roles = await _userManager.GetRolesAsync(currentGuidId);

            return Ok(roles);
        }
        [Route("api/role")]
        [HttpPost]
        public async Task<IHttpActionResult> Create(RoleCreateBindingModel model)
        {
            var newRole = new IdentityRole { Name = model.Name };

            var result = await _roleManager.CreateAsync(newRole);

            return Ok(result);
        }
    }
}

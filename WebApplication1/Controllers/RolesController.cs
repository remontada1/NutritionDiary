using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication1.Identity;
using WebApplication1.Models;


namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : ApiController
    {
        private readonly UserManager<ApplicationUser, Guid> userManager;
        private readonly RoleStore<IdentityRole> roleStore;
        private readonly ApplicationRoleManager roleManager;

        public RolesController(RoleStore<IdentityRole> roleStore, ApplicationRoleManager roleManager,
            UserManager<ApplicationUser, Guid> userManager)
        {
            this.userManager = userManager;
            this.roleStore = roleStore;
            this.roleManager = roleManager;
        }
        [Route("api/role/{id:guid}")]
        public async Task<IHttpActionResult> GetRole(string id)
        {
            var role = roleManager.FindByIdAsync(id);


            if (role != null)
            {
                return Ok(role);
            }

            return NotFound();
        }

        [Route("api/roles")]
        public IHttpActionResult GetAllRoles()
        {
            var roles = roleManager.Roles;
            return Ok(roles);
        }
        [HttpPost]
        [Route("api/role")]
        public async Task<IHttpActionResult> Create(CreateRoleBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = new IdentityRole { Name = model.Name };

            var result = await roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                return NotFound();
            }

            Uri locationHeader = new Uri(Url.Link("api/role/{id}", new { id = role.Id }));

            return Created(locationHeader, result);
        }

        [HttpDelete]
        [Route("api/role/{id:guid}")]
        public async Task<IHttpActionResult> DeleteRole(string id)
        {

            var role = await roleManager.FindByIdAsync(id);

            if (role != null)
            {
                IdentityResult result = await roleManager.DeleteAsync(role);

                if (!result.Succeeded)
                {
                    return NotFound();
                }

                return Ok();
            }

            return NotFound();

        }

        [Route("api/roles/manageUsers")]
        public async Task<IHttpActionResult> ManageUsersInRole(UsersInModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ModelState.AddModelError("", "Role does not exist");
                return BadRequest(ModelState);
            }

            foreach (string user in model.EnrolledUsers)
            {
                var ownerIdGuid = Guid.Parse(user);

                var appUser = await userManager.FindByIdAsync(ownerIdGuid);

                if (appUser == null)
                {
                    ModelState.AddModelError("", String.Format("User: {0} does not exists", user));
                    continue;
                }

                if (!userManager.IsInRole(ownerIdGuid, role.Name))
                {
                    IdentityResult result = await userManager.AddToRoleAsync(ownerIdGuid, role.Name);

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", String.Format("User: {0} could not be added to role", user));
                    }

                }
            }

            foreach (string user in model.RemovedUsers)
            {
                var ownerIdGuid = Guid.Parse(user);
                var appUser = await userManager.FindByIdAsync(ownerIdGuid);

                if (appUser == null)
                {
                    ModelState.AddModelError("", String.Format("User: {0} does not exists", user));
                    continue;
                }

                IdentityResult result = await userManager.RemoveFromRoleAsync(ownerIdGuid, role.Name);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", String.Format("User: {0} could not be removed from role", user));
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}

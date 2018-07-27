using System;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApplication1.Identity;
using WebApplication1.Models;
using System.Threading.Tasks;
using System.Net;
using WebApplication1.Repository;
using WebApplication1.Service;
using System.Web.Http.Cors;
using System.Linq;
using WebApplication1.Infrastructure;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security;

namespace WebApplication1.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AccountController : ApiController
    {
        private readonly RoleManager<Identity.IdentityRole, Guid> _roleManager;
        private readonly UserManager<Identity.ApplicationUser, Guid> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IMealService _mealService;
        private readonly IRoleRepository _roleRepository;


        public AccountController(UserManager<ApplicationUser, Guid> userManager,
            IUserRepository userRepository, IMealService mealService,
            IUnitOfWork unitOfWork, IRoleRepository roleRepository)
        {

            _roleRepository = roleRepository;
            _userManager = userManager;
            _userRepository = userRepository;
            _mealService = mealService;
        }


        [Authorize]
        [Route("api/test")]
        public IEnumerable<object> Get()
        {
            var identity = User.Identity as ClaimsIdentity;

            return identity.Claims.Select(c => new
            {
                c.Type,
                c.Value
            });
        }

        [AllowAnonymous]
        [Route("signup")]
        public async Task<IHttpActionResult> Register(UserBindingModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.NotAcceptable, "Input is not valid");
            }

            var user = new ApplicationUser() { UserName = userModel.UserName, JoinDate = new DateTime(2010, 08, 08) };
            var result = await _userManager.CreateAsync(user, userModel.Password);

            if (result.Succeeded)
            {
                return Content(HttpStatusCode.OK, "User created");
            }
            else
            {
                return Content(HttpStatusCode.NotModified, "User not created");
            }
        }

        [Authorize]
        [Route("api/info")]
        public IHttpActionResult GetUserInfo()
        {
            Guid ownerIdGuid = Guid.Empty; // create Guid for further converting

            var currentUserId = User.Identity.GetUserId();  //get current ID
            var currentGuidId = new Guid(currentUserId);  // convert to Guid type

            User currentUser = _userRepository.FindByGuid(currentGuidId);

            var mealList = _mealService.GetCurrentUserMeal();

            return Content(HttpStatusCode.OK, mealList);
        }


        [Route("api/roles/{id:guid}/{role}")]
        [HttpGet]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] Guid id, [FromUri] string role)
        {
            var result = await _userManager.AddToRoleAsync(id, role);
            string message = String.Format("Role {0} assigned to user", role);

            return Content(HttpStatusCode.OK, message);
        }



        private Guid getGuid(string value)
        {
            Guid.TryParse(value, out Guid result);
            return result;
        }
    }
}

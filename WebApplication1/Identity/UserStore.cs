using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using WebApplication1.Infrastructure;
using System.Threading.Tasks;
using WebApplication1.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.SqlServer.Utilities;
using System.Globalization;

namespace WebApplication1.Identity
{
    public class UserStore :
        IUserLoginStore<ApplicationUser, Guid>, IUserClaimStore<ApplicationUser, Guid>,
        IUserRoleStore<ApplicationUser, Guid>, IUserPasswordStore<ApplicationUser, Guid>,
        IUserSecurityStampStore<ApplicationUser, Guid>, IUserStore<ApplicationUser, Guid>, IDisposable
    {

        private readonly IUnitOfWork _unitOfWork;
        protected internal IUserStore<ApplicationUser, Guid> Store { get; set; }
        private IPasswordHasher _passwordHasher;
        public UserStore(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IPasswordHasher PasswordHasher
        {
            get
            {

                return _passwordHasher;
            }
            set
            {

                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _passwordHasher = value;
            }
        }

        #region IUserStore<ApplicationUser, Guid> Members
        public Task CreateAsync(Identity.ApplicationUser appUser)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                UserName = appUser.UserName,
                PasswordHash = appUser.PasswordHash,
                SecurityStamp = appUser.SecurityStamp

            };

            _unitOfWork.UserRepository.Add(user);
            return _unitOfWork.CommitAsync();
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException("User not found.");

            var myUser = getUser(user);

            _unitOfWork.UserRepository.RemoveByEntity(myUser);
            return _unitOfWork.CommitAsync();
        }
        public Task<ApplicationUser> FindAsync(UserLoginInfo login)
        {
            if (login == null)
                throw new ArgumentNullException("login");

            var applicationUser = default(ApplicationUser);

            var l = _unitOfWork.ExternalLoginRepository.GetByProviderAndKey(login.LoginProvider, login.ProviderKey);
            if (l != null)
                applicationUser = getApplicationUser(l.User);

            return Task.FromResult<ApplicationUser>(applicationUser);
        }

        public Task<ApplicationUser> FindByIdAsync(Guid userId)
        {
            var user = _unitOfWork.UserRepository.FindById(userId);
            return Task.FromResult<ApplicationUser>(getApplicationUser(user));
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            var user = _unitOfWork.UserRepository.FindByUsername(userName);
            return Task.FromResult<ApplicationUser>(getApplicationUser(user));
        }
        public virtual async Task<ApplicationUser> FindAsync(string userName, string password)
        {

            var user = await FindByNameAsync(userName);
            if (user == null)
            {
                return null;
            }
            return await CheckPasswordAsync(user, password).WithCurrentCulture() ? user : null;
        }


        public virtual async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {

            var passwordStore = GetPasswordStore();
            if (user == null)
            {
                return false;
            }
            return await VerifyPasswordAsync(passwordStore, user, password).WithCurrentCulture();
        }


        protected virtual async Task<bool> VerifyPasswordAsync(IUserPasswordStore<ApplicationUser, Guid> store, ApplicationUser user,
            string password)
        {
            var hash = await store.GetPasswordHashAsync(user).WithCurrentCulture();
            return PasswordHasher.VerifyHashedPassword(hash, password) != PasswordVerificationResult.Failed;
        }

        private IUserPasswordStore<ApplicationUser, Guid> GetPasswordStore()
        {
            var cast = Store as IUserPasswordStore<ApplicationUser, Guid>;
            if (cast == null)
            {
                throw new NotSupportedException();
            }
            return cast;
        }


        public virtual async Task<bool> HasPasswordAsync(Guid userId)
        {

            var passwordStore = GetPasswordStore();
            var user = await FindByIdAsync(userId).WithCurrentCulture();
            if (user == null)
            {
                throw new InvalidOperationException();
            }
            return await passwordStore.HasPasswordAsync(user).WithCurrentCulture();
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentException("user");

            var localUser = _unitOfWork.UserRepository.FindById(user.Id);
            if (localUser == null)
                throw new ArgumentException("ApplicationUser does not correspond to a User entity.", "user");

            populateUser(localUser, user);

            _unitOfWork.UserRepository.Update(localUser);
            return _unitOfWork.CommitAsync();
        }

        public Task AddLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (login == null)
                throw new ArgumentNullException("login");

            var myUser = _unitOfWork.UserRepository.FindById(user.Id);
            if (myUser == null)
                throw new ArgumentException("ApplicationUser does not correspond to a User entity.", "user");

            var myLogin = new ExternalLogin
            {
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey,
                User = myUser
            };
            myUser.Logins.Add(myLogin);

            _unitOfWork.UserRepository.Update(myUser);
            return _unitOfWork.CommitAsync();
        }
        public Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("ApplicationUser does not correspond to a User entity.", "user");

            return Task.FromResult<IList<UserLoginInfo>>(u.Logins.Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey)).ToList());
        }

        public Task RemoveLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (login == null)
                throw new ArgumentNullException("login");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("ApplicationUser does not correspond to a User entity.", "user");

            var l = u.Logins.FirstOrDefault(x => x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey);
            u.Logins.Remove(l);

            _unitOfWork.UserRepository.Update(u);
            return _unitOfWork.CommitAsync();
        }
        #endregion

        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult<string>(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult<bool>(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        #region IUserClaimStore<ApplicationUser, Guid> Members
        public Task AddClaimAsync(ApplicationUser user, System.Security.Claims.Claim claim)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (claim == null)
                throw new ArgumentNullException("claim");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("ApplicationUser does not correspond to a User entity.", "user");

            var c = new Models.Claim
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                User = u
            };
            u.Claims.Add(c);

            _unitOfWork.UserRepository.Update(u);
            return _unitOfWork.CommitAsync();
        }

        public Task<IList<System.Security.Claims.Claim>> GetClaimsAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("ApplicationUser does not correspond to a User entity.", "user");

            return Task.FromResult<IList<System.Security.Claims.Claim>>(u.Claims.Select(x => new System.Security.Claims.Claim(x.ClaimType, x.ClaimValue)).ToList());
        }

        public Task RemoveClaimAsync(ApplicationUser user, System.Security.Claims.Claim claim)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (claim == null)
                throw new ArgumentNullException("claim");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("ApplicationUser does not correspond to a User entity.", "user");

            var c = u.Claims.FirstOrDefault(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);
            u.Claims.Remove(c);

            _unitOfWork.UserRepository.Update(u);
            return _unitOfWork.CommitAsync();
        }
        #endregion


        public Task AddToRoleAsync(ApplicationUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: roleName.");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("ApplicationUser does not correspond to a User entity.", "user");
            var r = _unitOfWork.RoleRepository.FindByName(roleName);
            if (r == null)
                throw new ArgumentException("roleName does not correspond to a Role entity.", "roleName");

            u.Roles.Add(r);
            _unitOfWork.UserRepository.Update(u);

            return _unitOfWork.CommitAsync();
        }

        public Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("ApplicationUser does not correspond to a User entity.", "user");

            return Task.FromResult<IList<string>>(u.Roles.Select(x => x.Name).ToList());
        }

        public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: role.");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("ApplicationUser does not correspond to a User entity.", "user");

            return Task.FromResult<bool>(u.Roles.Any(x => x.Name == roleName));
        }

        public Task RemoveFromRoleAsync(ApplicationUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: role.");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("ApplicationUser does not correspond to a User entity.", "user");

            var r = u.Roles.FirstOrDefault(x => x.Name == roleName);
            u.Roles.Remove(r);

            _unitOfWork.UserRepository.Update(u);
            return _unitOfWork.CommitAsync();
        }
        #region IDisposable Members
        public void Dispose()
        {
            // Dispose does nothing since we want Unity to manage the lifecycle of our Unit of Work
        }
        #endregion

        #region Private Methods
        private User getUser(ApplicationUser applicationUser)
        {
            if (applicationUser == null)
                return null;

            var user = new User();
            populateUser(user, applicationUser);

            return user;
        }

        private void populateUser(User user, ApplicationUser applicationUser)
        {

            user.UserName = applicationUser.UserName;

        }

        private ApplicationUser getApplicationUser(User user)
        {
            if (user == null)
                return null;

            var applicationUser = new ApplicationUser();
            populateApplicationUser(applicationUser, user);

            return applicationUser;
        }

        private void populateApplicationUser(ApplicationUser applicationUser, Models.User user)
        {
            applicationUser.Id = user.Id;
            applicationUser.UserName = user.UserName;
            applicationUser.PasswordHash = user.PasswordHash;
            applicationUser.SecurityStamp = user.SecurityStamp;
        }

        public Task<string> GetSecurityStampAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult<string>(user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(ApplicationUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }
        #endregion
    }
}
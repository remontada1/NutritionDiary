using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

using WebApplication1.Infrastructure;
using System.Threading.Tasks;
using WebApplication1.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.SqlServer.Utilities;
using System.Globalization;

namespace WebApplication1.Identity
{
    public class UserStore :
        IUserLoginStore<ApplicationUser, Guid>, IUserStore<ApplicationUser, Guid>,
        IUserPasswordStore<ApplicationUser, Guid>, IDisposable
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
        #region IUserStore<IdentityUser, Guid> Members
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

            var identityUser = default(ApplicationUser);

            var l = _unitOfWork.ExternalLoginRepository.GetByProviderAndKey(login.LoginProvider, login.ProviderKey);
            if (l != null)
                identityUser = getIdentityUser(l.User);

            return Task.FromResult<ApplicationUser>(identityUser);
        }

        public Task<ApplicationUser> FindByIdAsync(Guid userId)
        {
            var user = _unitOfWork.UserRepository.FindById(userId);
            return Task.FromResult<ApplicationUser>(getIdentityUser(user));
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            var user = _unitOfWork.UserRepository.FindByUsername(userName);
            return Task.FromResult<ApplicationUser>(getIdentityUser(user));
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
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

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
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

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
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

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
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

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

        #region IDisposable Members
        public void Dispose()
        {
            // Dispose does nothing since we want Unity to manage the lifecycle of our Unit of Work
        }
        #endregion




        #region Private Methods
        private User getUser(ApplicationUser identityUser)
        {
            if (identityUser == null)
                return null;

            var user = new User();
            populateUser(user, identityUser);

            return user;
        }

        private void populateUser(User user, ApplicationUser identityUser)
        {

            user.UserName = identityUser.UserName;

        }

        private ApplicationUser getIdentityUser(User user)
        {
            if (user == null)
                return null;

            var identityUser = new ApplicationUser();
            populateIdentityUser(identityUser, user);

            return identityUser;
        }

        private void populateIdentityUser(ApplicationUser identityUser, Models.User user)
        {
            identityUser.Id = user.Id;
            identityUser.UserName = user.UserName;
            identityUser.PasswordHash = user.PasswordHash;
            identityUser.SecurityStamp = user.SecurityStamp;
        }


        #endregion
    }
}
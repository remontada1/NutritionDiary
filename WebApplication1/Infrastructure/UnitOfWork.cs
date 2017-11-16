using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DAL;
using WebApplication1.Repository;

namespace WebApplication1.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory dbFactory;
        private CustomerContext dbContext;
        private IUserRepository _userRepository;
        private IExternalLoginRepository _externalLoginRepository;
        public UnitOfWork(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public CustomerContext DbContext
        {
            get { return dbContext ?? (dbContext = dbFactory.Init()); }
        }

        public IUserRepository UserRepository
        {
            get { return _userRepository ?? (_userRepository = new UserRepository(dbFactory)); }
        }

        public IExternalLoginRepository ExternalLoginRepository
        {
            get { return _externalLoginRepository ?? (_externalLoginRepository = new ExternalLoginRepository(dbFactory)); }
        }
        public void Commit()
        {
             DbContext.Commit();
        }

        public Task<int> CommitAsync()
        {
          try
        {
            return DbContext.CommitAsync();
        }
        catch (DbEntityValidationException ex)
        {
            var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

            var fullErrorMessage = string.Join("; ", errorMessages);

            var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

            throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
        }
        }


    }
}
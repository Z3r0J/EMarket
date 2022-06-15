using EMarket.Core.Application.Interfaces.Repository;
using EMarket.Core.Domain.Entities;
using EMarket.Infrastructure.Persistence.Contexts;

namespace EMarket.Infrastructure.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>,IUserRepository
    {
        private readonly ApplicationContext _applicationContext;

        public UserRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
            _applicationContext = applicationContext;
        }
    }
}

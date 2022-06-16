using EMarket.Core.Application.Helpers;
using EMarket.Core.Application.Interfaces.Repository;
using EMarket.Core.Application.ViewModel.User;
using EMarket.Core.Domain.Entities;
using EMarket.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EMarket.Infrastructure.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>,IUserRepository
    {
        private readonly ApplicationContext _applicationContext;

        public UserRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
            _applicationContext = applicationContext;
        }


        public override async Task<User> AddAsync(User entity) {
            entity.Password = PasswordEncryption.ComputeSHA256Hash(entity.Password);

            await base.AddAsync(entity);

            return entity;

        }


        public async Task<User> LoginAsync(LoginViewModel vm) {

            string PasswordEncrypted = PasswordEncryption.ComputeSHA256Hash(vm.Password);

            User user = await _applicationContext.Set<User>().FirstOrDefaultAsync(user => user.Username == vm.Username && user.Password == PasswordEncrypted);


            return user;

        }

    }
}

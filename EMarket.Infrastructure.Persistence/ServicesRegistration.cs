using EMarket.Core.Application.Interfaces.Repository;
using EMarket.Infrastructure.Persistence.Contexts;
using EMarket.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMarket.Infrastructure.Persistence
{
    public static class ServicesRegistration
    {
        public static void AddPersistanceInfrastructure(this IServiceCollection services, IConfiguration configuration) {

            #region Contexts

            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationContext>(options=>options.UseInMemoryDatabase("EMarketMemory"));
            }
            else {
                services.AddDbContext<ApplicationContext>(options => 
                options.UseSqlServer(configuration.GetConnectionString("EMarketDatabaseConnection"),
                m=>m.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
            }
            #endregion

            #region Repositories

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IUserRepository, UserRepository>();

            #endregion
        }
    }
}

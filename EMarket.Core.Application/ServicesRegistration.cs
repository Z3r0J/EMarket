using EMarket.Core.Application.Interfaces.Services;
using EMarket.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMarket.Core.Application
{
    public static class ServicesRegistration
    {

        public static void AddApplicationLayer(this IServiceCollection service) {

            #region Services

            service.AddTransient<IUserServices,UserServices>();
            service.AddTransient<IAdvertisingServices,AdvertisingServices>();

            #endregion

        }

    }
}

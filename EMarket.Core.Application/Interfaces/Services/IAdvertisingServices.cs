using EMarket.Core.Application.ViewModel.Advertising;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMarket.Core.Application.Interfaces.Services
{
    public interface IAdvertisingServices:IGenericServices<SaveAdvertisingViewModel,AdvertisingViewModel>
    {
        Task<List<AdvertisingViewModel>> FilterByCategory(List<int?> categories);
        Task<DetailsAdvertisingViewModel> GetDetailsAdvertising(int id);
        Task<List<AdvertisingViewModel>> GetAdvertisingByName(string Name);
    }
}

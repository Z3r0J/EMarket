using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMarket.Core.Application.Interfaces.Services
{
    public interface IGenericServices<SaveViewModel,ViewModel> 
        where SaveViewModel : class 
        where ViewModel : class
    {
        Task<SaveViewModel> Add(SaveViewModel vm);
        Task<SaveViewModel> GetByIdSaveViewModel(int id);

        Task<List<ViewModel>> GetAllViewModel();

        Task Update(SaveViewModel vm);
        Task Delete(int id);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMarket.Core.Application.Interfaces.Services
{
    public interface IGenericTwo<SaveViewModel,ViewModel> 
        where SaveViewModel : class 
        where ViewModel : class
    {
        Task<SaveViewModel> Add(SaveViewModel vm);
        Task Update(SaveViewModel vm);
    }
}

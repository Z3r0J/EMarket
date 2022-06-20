using EMarket.Core.Application.ViewModel.User;
using System.Threading.Tasks;

namespace EMarket.Core.Application.Interfaces.Services
{
    public interface IUserServices:IGenericServices<SaveUserViewModel,UserViewModel>
    {
        Task<UserViewModel> Login(LoginViewModel vm);
        Task<bool> CheckUserName(string username);
    }
}

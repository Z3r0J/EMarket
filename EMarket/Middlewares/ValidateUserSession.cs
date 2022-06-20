using EMarket.Core.Application.Helpers;
using EMarket.Core.Application.ViewModel.User;
using Microsoft.AspNetCore.Http;

namespace WebApp.EMarket.Middlewares
{
    public class ValidateUserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ValidateUserSession(IHttpContextAccessor httpContextAccesor)
        {
            _httpContextAccessor = httpContextAccesor;
        }

        public bool HasUser() {

            UserViewModel vm = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("userEmarket");

            return vm == null ? false : true;
        }
    }
}

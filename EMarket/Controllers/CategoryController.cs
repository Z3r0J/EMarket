using EMarket.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.EMarket.Middlewares;

namespace WebApp.EMarket.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryServices _categoryServices;
        private readonly ValidateUserSession _validateUserSession;
        public CategoryController(ICategoryServices categoryServices, ValidateUserSession validateUserSession)
        {
            _categoryServices = categoryServices;
            _validateUserSession = validateUserSession;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _categoryServices.GetAllViewModel();

            return _validateUserSession.HasUser() ? View(response) : RedirectToRoute(new { action="Index",controller="User"});
        }
    }
}

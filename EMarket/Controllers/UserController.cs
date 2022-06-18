using EMarket.Core.Application.Interfaces.Services;
using EMarket.Core.Application.ViewModel.User;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApp.EMarket.Controllers
{
    public class UserController : Controller
    {

        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var session = await _userServices.Login(vm);

            if (session == null)
            {
                return View(vm);
            }

            return RedirectToRoute(new { action = "Index", controller = "Home" });
        }
    }
}

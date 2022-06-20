using EMarket.Core.Application.Helpers;
using EMarket.Core.Application.Interfaces.Services;
using EMarket.Core.Application.ViewModel.User;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.EMarket.Middlewares;

namespace WebApp.EMarket.Controllers
{
    public class UserController : Controller
    {

        private readonly IUserServices _userServices;
        private readonly ValidateUserSession _validateUserSession;
        public UserController(IUserServices userServices, ValidateUserSession validateUserSession)
        {
            _userServices = userServices;
            _validateUserSession = validateUserSession;
        }

        public IActionResult Index()
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {

            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            var session = await _userServices.Login(vm);
            if (session != null)
            {
                HttpContext.Session.Set<UserViewModel>("userEmarket", session);
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            else
            {
                ModelState.AddModelError("userValidation", "Datos de acceso incorrecto");
            }

            return View(vm);
        }

        public IActionResult LogOut() {

            HttpContext.Session.Remove("userEmarket");

            return RedirectToRoute(new {action="Index",controller="User" });
        
        }

        public IActionResult Register() {

            return _validateUserSession.HasUser() ? RedirectToRoute(new { controller="Home", action = "Index" }) : View(new SaveUserViewModel());
        
        }

        [HttpPost]
        public async Task<IActionResult> Register(SaveUserViewModel vm) {

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new {controller="Home",action="Index" });
            }

            if (await _userServices.CheckUserName(vm.Username))
            {
                ModelState.AddModelError("Username", "The username has been taken, insert another one.");
                return View(vm);
            }

            await _userServices.Add(vm);

            return RedirectToRoute(new { controller="User",action="Index"});
        
        }
    }
}

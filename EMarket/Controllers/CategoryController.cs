using EMarket.Core.Application.Interfaces.Services;
using EMarket.Core.Application.ViewModel.Category;
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

            return _validateUserSession.HasUser() ? View(response) : RedirectToRoute(new { action = "Index", controller = "User" });
        }

        public IActionResult Create() {

            return _validateUserSession.HasUser() ? View("SaveCategory", new SaveCategoryViewModel()) : RedirectToRoute(new { action = "Index", controller = "User" });
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveCategoryViewModel vm)
        {

            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { action = "Index", controller = "User" });
            }

            if (!ModelState.IsValid)
            {
                return View("SaveCategory", vm);
            }

            await _categoryServices.Add(vm);

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Edit(int Id) {


            return _validateUserSession.HasUser() ? View("SaveCategory", await _categoryServices.GetByIdSaveViewModel(Id)) : RedirectToRoute(new { action = "Index", controller = "User" });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(SaveCategoryViewModel vm) {

            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { action = "Index", controller = "User" });
            }

            if (!ModelState.IsValid)
            {
                return View("SaveCategory", vm);
            }

            await _categoryServices.Update(vm);

            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Delete(int Id)
        {
            return _validateUserSession.HasUser() ? View(await _categoryServices.GetByIdSaveViewModel(Id)) : RedirectToRoute(new { action = "Index", controller = "User" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SaveCategoryViewModel vm) {

            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { action = "Index", controller = "User" });
            }

            await _categoryServices.Delete(vm.Id);

            return RedirectToAction("Index");
        
        }
    }
}

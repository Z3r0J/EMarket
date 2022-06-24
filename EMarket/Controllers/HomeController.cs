using EMarket.Core.Application.Helpers;
using EMarket.Core.Application.Interfaces.Services;
using EMarket.Core.Application.ViewModel.Advertising;
using EMarket.Core.Application.ViewModel.User;
using EMarket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApp.EMarket.Middlewares;

namespace EMarket.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAdvertisingServices _advertisingServices;
        private readonly ValidateUserSession _validateUserSession;
        private readonly ICategoryServices _categoryServices;

        private FilterByCategory filter = new();
        public HomeController(IAdvertisingServices advertisingServices, ValidateUserSession validateUserSession, ICategoryServices categoryServices)
        {
            _validateUserSession = validateUserSession;
            _advertisingServices = advertisingServices;
            _categoryServices = categoryServices;
        }

        public async Task<IActionResult> Index()
        {
            var advertising = await _advertisingServices.GetAllViewModel();

            var user = HttpContext.Session.Get<UserViewModel>("userEmarket");

            ViewBag.Categories = await _categoryServices.GetAllViewModel();

            return _validateUserSession.HasUser() ? View(advertising.Where(advertising => advertising.User != user.Username).ToList()) : RedirectToRoute(new { action = "Index", controller = "User" });
        }

        [HttpPost]
        public async Task<IActionResult> Index(FilterByCategory vm) {
            var user = HttpContext.Session.Get<UserViewModel>("userEmarket");

            if (!string.IsNullOrEmpty(vm.Name)) {

                var response = await _advertisingServices.GetAdvertisingByName(vm.Name);
                

                ViewBag.Categories = await _categoryServices.GetAllViewModel();

                return View(response.Where(x=>x.User!=user.Username).ToList());
            }

            if (vm.CategoryId == null||vm.CategoryId.Count==0) {

                var response = await _advertisingServices.GetAllViewModel();
                ViewBag.Categories = await _categoryServices.GetAllViewModel();

                return View(response.Where(advertising=>advertising.User!=user.Username).ToList());

            }

            var advertising = await _advertisingServices.FilterByCategory(vm.CategoryId);

            ViewBag.Categories = await _categoryServices.GetAllViewModel();

            return View(advertising.Where(advertising=>advertising.User!=user.Username).ToList());
        }
    }
}

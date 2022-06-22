using EMarket.Core.Application.Interfaces.Services;
using EMarket.Models;
using Microsoft.AspNetCore.Mvc;
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
        public readonly IAdvertisingServices _advertisingServices;
        public readonly ValidateUserSession _validateUserSession;
        public HomeController(IAdvertisingServices advertisingServices,ValidateUserSession validateUserSession)
        {
            _validateUserSession = validateUserSession;
            _advertisingServices = advertisingServices;
        }

        public async Task<IActionResult> Index()
        {
            var advertising = await _advertisingServices.GetAllViewModel();

            return _validateUserSession.HasUser() ? View(advertising) : RedirectToRoute(new { action = "Index", controller = "User" });
        }

    }
}

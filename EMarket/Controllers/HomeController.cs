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

        public readonly ValidateUserSession _validateUserSession;
        public HomeController(ValidateUserSession validateUserSession)
        {
            _validateUserSession = validateUserSession;
        }

        public IActionResult Index()
        {
            return _validateUserSession.HasUser() ? View() : RedirectToRoute(new { action = "Index", controller = "User" });
        }

    }
}

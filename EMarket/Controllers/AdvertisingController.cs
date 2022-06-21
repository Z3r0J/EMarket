using EMarket.Core.Application.Helpers;
using EMarket.Core.Application.Interfaces.Repository;
using EMarket.Core.Application.Interfaces.Services;
using EMarket.Core.Application.ViewModel.Advertising;
using EMarket.Core.Application.ViewModel.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using WebApp.EMarket.Middlewares;

namespace WebApp.EMarket.Controllers
{
    public class AdvertisingController : Controller
    {
        private readonly IAdvertisingServices _advertisingServices;
        private readonly ValidateUserSession _validateUserSession;
        public AdvertisingController(IAdvertisingServices advertisingServices,ValidateUserSession validateUserSession)
        {
            _advertisingServices = advertisingServices;
            _validateUserSession = validateUserSession;
        }
        public IActionResult Index()
        {
            return _validateUserSession.HasUser() ? View() : RedirectToRoute(new {action="Index",controller="User"});
        }

        public IActionResult Create() {

            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { action = "Index",controller="User" });
            }

            return View("SaveAdvertising",new SaveAdvertisingViewModel());

        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveAdvertisingViewModel vm) {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { action = "Index", controller = "User" });
            }

            if (!ModelState.IsValid)
            {
                return View("SaveAdvertising",vm);
            }

            if (vm.Photos.Count>4)
            {
                ModelState.AddModelError("Photos", "No more than 1 to 4 photos please.");

                return View("SaveAdvertising",vm);
            }

            vm.UserId = HttpContext.Session.Get<UserViewModel>("userEmarket").Id;
            vm.PrincipalPhoto = "/";

            var response = await _advertisingServices.Add(vm);

            response.Gallery = new();

            if (response.Id != 0 && response != null)
            {
                foreach (var file in vm.Photos)
                {
                    
                    response.Gallery.Add(new() { 
                    Name= file.FileName,
                    Url = UploadFile(file,response.Id),
                    AdvertisingId = response.Id
                    });
                }

                response.PrincipalPhoto = response.Gallery[0].Url;

                await _advertisingServices.Update(response);
            }

            return RedirectToAction("Index");

        }

        private string UploadFile(IFormFile file, int id, bool isEditMode = false, string ImagePath = "") {

            if (isEditMode)
            {
                if (file == null)
                {
                    return ImagePath;
                }
            }
            string basePath = $"/Images/Advertising/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            //create folder if not exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //get file extension
            Guid guid = Guid.NewGuid();
            FileInfo fileInfo = new(file.FileName);
            string fileName = guid + fileInfo.Extension;

            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            if (isEditMode)
            {
                string[] oldImagePart = ImagePath.Split("/");
                string oldImagePath = oldImagePart[^1];
                string completeImageOldPath = Path.Combine(path, oldImagePath);

                if (System.IO.File.Exists(completeImageOldPath))
                {
                    System.IO.File.Delete(completeImageOldPath);
                }
            }
            return $"{basePath}/{fileName}";

        }
    }
}

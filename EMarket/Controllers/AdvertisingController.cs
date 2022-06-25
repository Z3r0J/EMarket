using EMarket.Core.Application.Helpers;
using EMarket.Core.Application.Interfaces.Repository;
using EMarket.Core.Application.Interfaces.Services;
using EMarket.Core.Application.ViewModel.Advertising;
using EMarket.Core.Application.ViewModel.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApp.EMarket.Middlewares;

namespace WebApp.EMarket.Controllers
{
    public class AdvertisingController : Controller
    {
        private readonly IAdvertisingServices _advertisingServices;
        private readonly ValidateUserSession _validateUserSession;
        private readonly ICategoryServices _categoryServices;
        public AdvertisingController(IAdvertisingServices advertisingServices,ValidateUserSession validateUserSession,ICategoryServices categoryServices)
        {
            _advertisingServices = advertisingServices;
            _validateUserSession = validateUserSession;
            _categoryServices = categoryServices;
        }
        public async Task<IActionResult> Index()
        {
            string UserName = HttpContext.Session.Get<UserViewModel>("userEmarket").Username;
            List<AdvertisingViewModel> list = await _advertisingServices.GetAllViewModel();

            return _validateUserSession.HasUser() ? View(list.Where(x=>x.User==UserName).ToList()) : RedirectToRoute(new {action="Index",controller="User"});
        }

        public async Task<IActionResult> Create() {

            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { action = "Index",controller="User" });
            }

            return View("SaveAdvertising",new SaveAdvertisingViewModel() { Categories = await _categoryServices.GetAllViewModel(),Gallery=new()});

        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveAdvertisingViewModel vm) {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { action = "Index", controller = "User" });
            }

            if (!ModelState.IsValid)
            {
                vm.Gallery = new();
                vm.Categories = await _categoryServices.GetAllViewModel();
                return View("SaveAdvertising",vm);
            }

            if (vm.Photos.Count>4)
            {
                ModelState.AddModelError("Photos", "No more than 1 to 4 photos please.");
                vm.Gallery=new();
                vm.Categories = await _categoryServices.GetAllViewModel();
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

        public async Task<IActionResult> Edit(int id) {

            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { action = "Index", controller = "User" });
            }

            var response = await _advertisingServices.GetByIdSaveViewModel(id);

            int UserId = HttpContext.Session.Get<UserViewModel>("userEmarket").Id;

            response.Categories = await _categoryServices.GetAllViewModel();

            response.Gallery = response.Gallery;

            return response.UserId == UserId ? View("SaveAdvertising", response) : RedirectToRoute(new { action="Index",controller="Advertising" });
        
        }
        [HttpPost]
        public async Task<IActionResult> Edit(SaveAdvertisingViewModel vm) {

            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { action = "Index", controller = "User" });
            }
            if (!ModelState.IsValid)
            {
                vm.Gallery = new();
                vm.Categories = await _categoryServices.GetAllViewModel();

                return View("SaveAdvertising",vm);
            }
            if (vm.Photos!=null)
            {
                if (vm.Photos.Count>=0&&vm.Photos.Count<=4)
                {

                    vm.Gallery = new();
                    foreach (var file in vm.Photos)
                    {

                        vm.Gallery.Add(new()
                        {
                            Name = file.FileName,
                            Url = UploadFile(file,vm.Id,true,vm.PrincipalPhoto),
                            AdvertisingId = vm.Id
                        });
                    }

                    vm.PrincipalPhoto = vm.Gallery[0].Url;
                }
                if (vm.Photos.Count > 4)
                {
                    vm.Gallery = new();
                    vm.Categories = await _categoryServices.GetAllViewModel();
                    ModelState.AddModelError("Photos", "No more than 1 to 4 photos please.");

                    return View("SaveAdvertising", vm);
                }

            }

            vm.UserId = HttpContext.Session.Get<UserViewModel>("userEmarket").Id;

            await _advertisingServices.Update(vm);

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Delete(int id) {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            var response = await _advertisingServices.GetByIdSaveViewModel(id);

            return response.UserId == HttpContext.Session.Get<UserViewModel>("userEmarket").Id ? View(response) : RedirectToAction("Index");

        }
        [HttpPost]
        public async Task<IActionResult> DeletePost(SaveAdvertisingViewModel vm) {

            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }
            var gallery = await _advertisingServices.GetByIdSaveViewModel(vm.Id);

            await _advertisingServices.Delete(vm.Id);


            string basePath = $"/Images/Advertising/{vm.Id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            if (Directory.Exists(path))
            {
                DirectoryInfo directory = new(path);

                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo folder in directory.GetDirectories())
                {
                    folder.Delete(true);
                }

                Directory.Delete(path);
            }


            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Details(int id) {

            DetailsAdvertisingViewModel advertising = await _advertisingServices.GetDetailsAdvertising(id);

            return _validateUserSession.HasUser() ? View(advertising) : RedirectToRoute(new {action="Index",controller="User" });
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

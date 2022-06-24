using EMarket.Core.Application.Interfaces.Repository;
using EMarket.Core.Application.Interfaces.Services;
using EMarket.Core.Application.ViewModel.Category;
using EMarket.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMarket.Core.Application.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryServices(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<SaveCategoryViewModel> Add(SaveCategoryViewModel vm) {

            Category category = new() {Name = vm.Name,Description=vm.Description };
            var response = await _categoryRepository.AddAsync(category);

            return new()
            {
                Id = vm.Id,
                Name = vm.Name,
                Description = vm.Description
            };
        }

        public async Task<List<CategoryViewModel>> GetAllViewModel() { 
        
            var response = await _categoryRepository.GetAllWithIncludeAsync(new() { "Advertisings" });

            var userperadvertising = response
                .Select(x => new { Contador = x.Advertisings
                .GroupBy(adver => adver.UserId)
                .Distinct()
                .Count() });

            int UserCount = 0;

            foreach (var item in userperadvertising) {
                UserCount = item.Contador;
            }

            return response.Select(category => new CategoryViewModel() { 
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                AdvertisingQuantity = category.Advertisings.Count,
                UserPerAdvertising =UserCount
            }).ToList();

        }

        public async Task<SaveCategoryViewModel> GetByIdSaveViewModel(int id) {

            var response = await _categoryRepository.GetByIdAsync(id);

            return new() { 
            Id = response.Id,
            Name = response.Name,
            Description = response.Description
            };

        }

        public async Task Update(SaveCategoryViewModel vm) {

            var response = await _categoryRepository.GetByIdAsync(vm.Id);

            response.Id = vm.Id;
            response.Name = vm.Name;
            response.Description = vm.Description;

            await _categoryRepository.UpdateAsync(response);
        
        }

        public async Task Delete(int id) {
            var response = await _categoryRepository.GetByIdAsync(id);

            await _categoryRepository.DeleteAsync(response);

        }

    }
}

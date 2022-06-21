using EMarket.Core.Application.Interfaces.Repository;
using EMarket.Core.Application.Interfaces.Services;
using EMarket.Core.Application.ViewModel.Advertising;
using EMarket.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMarket.Core.Application.Services
{
    public class AdvertisingServices : IAdvertisingServices
    {
        private readonly IAdvertisingRepository _advertisingRepository;
        public AdvertisingServices(IAdvertisingRepository advertisingRepository)
        {
            _advertisingRepository = advertisingRepository;
        }

        public async Task<SaveAdvertisingViewModel> Add(SaveAdvertisingViewModel vm) {

            Advertising advertising = new() { 
            Name = vm.Name,
            Description = vm.Description,
            Price=vm.Price,
            PrincipalPhoto = vm.PrincipalPhoto,
            CategoryId = vm.CategoryId,
            UserId = vm.UserId
            };

            var response = await _advertisingRepository.AddAsync(advertising);

            SaveAdvertisingViewModel viewModel = new()
            {
                Id = response.Id,
                Name = response.Name,
                Description = response.Description,
                Price = response.Price,
                CategoryId = response.CategoryId,
                UserId = response.UserId,
                PrincipalPhoto = response.PrincipalPhoto
            };
            return viewModel;
        }

        public async Task Update(SaveAdvertisingViewModel vm) {

            Advertising advertising = await _advertisingRepository.GetByIdAsync(vm.Id);

            advertising.Id = vm.Id;
            advertising.Name = vm.Name;
            advertising.UserId = vm.UserId;
            advertising.Description = vm.Description;
            advertising.Price = vm.Price;
            advertising.CategoryId = vm.CategoryId;
            advertising.Gallery = vm.Gallery;
            advertising.PrincipalPhoto = vm.PrincipalPhoto;
            

            await _advertisingRepository.UpdateAsync(advertising);

        }
    }
}

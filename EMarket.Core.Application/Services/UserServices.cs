using EMarket.Core.Application.Interfaces.Repository;
using EMarket.Core.Application.Interfaces.Services;
using EMarket.Core.Application.ViewModel.User;
using EMarket.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMarket.Core.Application.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        public UserServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<SaveUserViewModel> Add(SaveUserViewModel vm) {

            User user = new() {
                Name = vm.Name,
                LastName = vm.LastName,
                Email = vm.Email,
                Phone = vm.Phone,
                Username = vm.Username,
                Password = vm.Password
            };

            user = await _userRepository.AddAsync(user);

            SaveUserViewModel userViewModel = new() {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Username = user.Username,
                Password = user.Password
            };

            return userViewModel;
        }

        public async Task<UserViewModel> Login(LoginViewModel vm) {

            User user = await _userRepository.LoginAsync(vm);

            UserViewModel userViewModel = new() { 
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone,
            Username = user.Username,
            Password = user.Password
            };

            return userViewModel;
        
        }

        public async Task<List<UserViewModel>> GetAllViewModel(){
            var userList = await _userRepository.GetAllAsync();

            return userList.Select(user => new UserViewModel() {
                
                Id = user.Id,
                Name = user.Name,
                LastName =  user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Username = user.Username,
                Password = user.Password
            }).ToList();
        }

        public async Task<SaveUserViewModel> GetByIdSaveViewModel(int id) {

            var user = await _userRepository.GetByIdAsync(id);

            SaveUserViewModel vm = new()
            {

                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Username = user.Phone,
                Password = user.Password
            };

            return vm;
        }

        public async Task Update(SaveUserViewModel vm)
        {
            User user = new() {
                Id = vm.Id,
                Name = vm.Name,
                LastName = vm.LastName,
                Email = vm.Email,
                Phone = vm.Phone,
                Username = vm.Username,
                Password = vm.Password            
            };

            await _userRepository.UpdateAsync(user);
        }

        public async Task Delete(int id) {

            var user = await _userRepository.GetByIdAsync(id);

            await _userRepository.DeleteAsync(user);

        }

    }
}

using AutoMapper;
using Base;
using Base.Exceptions;
using MongoDB.Bson;
using Repositories.ARepositories;
using Repositories.Entities;
using Repositories.ViewModels;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserRepository userRepository;
        private IMapper mapper;

        public UserService(UserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<bool> create(User user)
        {
            user.isActive = true;
            user.password = BCrypt.Net.BCrypt.HashPassword(user.password);
            return await userRepository.create(user);
        }

        public Task<string> createToken(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> delete(User user)
        {
            return await userRepository.softDelete(user);
        }

        public async Task<UserViewModel> findUserById(ObjectId id)
        {
            return mapper.Map<User, UserViewModel>(await userRepository.findById(id));
        }

        public async Task<IEnumerable<UserViewModel>> getAllUser()
        {
            var result = await userRepository.findAll();
            userRepository.Dispose();
            return mapper.Map<IEnumerable<User>, IEnumerable<UserViewModel>>(result);
        }

        public async Task<PageResult<UserViewModel>> getAllUserByPage(int page)
        {
            var result = await userRepository.findAllInPage(page);
            return mapper.Map<PageResult<User>, PageResult<UserViewModel>>(result);
        }

        public async Task<bool> hardDelete(User user)
        {
            return await userRepository.hardDelete(user);
        }

        public async Task<bool> loginUser()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> logoutUser()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> update(User user, string id)
        {
            var u = await userRepository.findById(new ObjectId(id));
            if (u == null) throw new NotFoundException($"User not found with id {id}");
            u.name = user.name;
            u.updatedAt = DateTime.Now;
            u.email = user.email;
            u.dob = user.dob;
            return await userRepository.update(u);
        }
    }
}

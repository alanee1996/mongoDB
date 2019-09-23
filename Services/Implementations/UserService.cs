using Base;
using Base.Exceptions;
using Repositories.ARepositories;
using Repositories.Entities;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserRepository userRepository;

        public UserService(UserRepository userRepository)
        {
            this.userRepository = userRepository;
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

        public async Task<User> findUserById(string id)
        {
            return await userRepository.findById(id);
        }

        public async Task<IEnumerable<User>> getAllUser()
        {
            var result = await userRepository.findAll();
            userRepository.Dispose();
            return result;
        }

        public async Task<PageResult<User>> getAllUserByPage(int page)
        {
            return await userRepository.findAllInPage(page);
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
            var u = await findUserById(id);
            if (u == null) throw new NotFoundException($"User not found with id {id}");
            u.name = user.name;
            u.updatedAt = DateTime.Now;
            u.email = user.email;
            u.dob = user.dob;
            return await userRepository.update(u);
        }
    }
}

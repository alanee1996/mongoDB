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

        public Task<string> createToken(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> findUserById()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> getAllUser()
        {
            return await userRepository.findAll();
        }

        public Task<bool> loginUser()
        {
            throw new NotImplementedException();
        }

        public Task<bool> logoutUser()
        {
            throw new NotImplementedException();
        }
    }
}

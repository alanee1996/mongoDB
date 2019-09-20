using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface IUserService
    {

        Task<IEnumerable<User>> getAllUser();
        Task<User> findUserById();
        Task<bool> loginUser();
        Task<bool> logoutUser();
        Task<string> createToken(User user);
    }
}

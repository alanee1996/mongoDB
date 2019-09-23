using Base;
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
        Task<PageResult<User>> getAllUserByPage(int page);
        Task<bool> create(User user);
        Task<bool> update(User user, string id);
        Task<bool> delete(User user);
        Task<bool> hardDelete(User user);
        Task<User> findUserById(string id);
        Task<bool> loginUser();
        Task<bool> logoutUser();
        Task<string> createToken(User user);
    }
}

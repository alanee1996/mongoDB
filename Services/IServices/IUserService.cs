using Base;
using MongoDB.Bson;
using Repositories.Entities;
using Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface IUserService
    {

        Task<IEnumerable<UserViewModel>> getAllUser();
        Task<PageResult<UserViewModel>> getAllUserByPage(int page);
        Task<bool> create(User user);
        Task<bool> update(User user, string id);
        Task<bool> delete(User user);
        Task<bool> hardDelete(User user);
        Task<UserViewModel> findUserById(ObjectId id);
        Task<bool> loginUser();
        Task<bool> logoutUser();
        Task<string> createToken(User user);
    }
}

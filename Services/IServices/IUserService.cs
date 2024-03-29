﻿using Base;
using MongoDB.Bson;
using Repositories.Entities;
using Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface IUserService : IDisposable
    {

        Task<IEnumerable<UserSummaryViewModel>> getAllUser();
        Task<PageResult<UserSummaryViewModel>> getAllUserByPage(int page);
        Task<bool> create(UserViewModel user);
        Task<bool> update(UserViewModel user, string id);
        Task<bool> delete(UserViewModel user);
        Task<bool> hardDelete(UserViewModel user);
        Task<UserSummaryViewModel> findUserById(ObjectId id);
        Task<IEnumerable<UserSummaryViewModel>> getUserExceptById(string id);
        Task<UserSummaryViewModel> loginUser(UserLoginViewModel user);
        Task<string> getUsernameById(string id);
        Task<bool> logoutUser();
        Task<string> createToken(User user);
        Task<UserSummaryViewModel> currentUser(ClaimsPrincipal principal);
    }
}

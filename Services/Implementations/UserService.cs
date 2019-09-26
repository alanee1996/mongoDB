using AutoMapper;
using Base;
using Base.Exceptions;
using Microsoft.Extensions.Options;
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
        private readonly AppSetting setting;
        public UserService(UserRepository userRepository, IMapper mapper, IOptions<AppSetting> option)
        {
            this.userRepository = userRepository;
            this.setting = option.Value;
            this.mapper = mapper;
        }

        public async Task<bool> create(UserViewModel user)
        {
            user.isActive = true;
            user.password = BCrypt.Net.BCrypt.HashPassword(user.password);
            return await userRepository.create(mapper.Map<UserViewModel, User>(user));
        }

        public Task<string> createToken(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> delete(UserViewModel user)
        {
            return await userRepository.softDelete(mapper.Map<UserViewModel, User>(user));
        }

        public async Task<UserViewModel> findUserById(ObjectId id)
        {
            return mapper.Map<User, UserViewModel>(await userRepository.findById(id));
        }

        public async Task<IEnumerable<UserSummaryViewModel>> getAllUser()
        {
            var result = await userRepository.findAll();
            userRepository.Dispose();
            return mapper.Map<IEnumerable<User>, IEnumerable<UserSummaryViewModel>>(result);
        }

        public async Task<PageResult<UserSummaryViewModel>> getAllUserByPage(int page)
        {
            var result = await userRepository.findAllInPage(page);
            return mapper.Map<PageResult<User>, PageResult<UserSummaryViewModel>>(result);
        }

        public async Task<bool> hardDelete(UserViewModel user)
        {
            return await userRepository.hardDelete(mapper.Map<UserViewModel, User>(user));
        }

        public async Task<UserSummaryViewModel> loginUser(UserLoginViewModel model)
        {
            var user = await userRepository.findUserByUsername(model.username);
            if (user == null) throw new UserNotFoundException("Username not found, please make sure the username is correct");
            if (Validator.isNullOrEmpty(model.password)) throw new InvalidDataException("Password cannot be null");
            if (!BCrypt.Net.BCrypt.Verify(model.password, user.password)) throw new InvalidDataException("Password incorrect");
            var result = mapper.Map<User, UserSummaryViewModel>(user);
            result.refreshToken = TokenHelper.createRefreshToken(user, setting.tokenKey, setting.refeshTokenDuration);
            result.accessToken = TokenHelper.createRefreshToken(user, setting.tokenKey, setting.accessTokenDuration);
            return result;
        }

        public async Task<bool> logoutUser()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> update(UserViewModel user, string id)
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

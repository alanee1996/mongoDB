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
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserRepository userRepository;
        private readonly RoleRepository roleRepositiry;
        private IMapper mapper;
        private readonly AppSetting setting;
        public UserService(UserRepository userRepository, IMapper mapper, IOptions<AppSetting> option, RoleRepository roleRepositiry)
        {
            this.userRepository = userRepository;
            this.roleRepositiry = roleRepositiry;
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

        public async Task<UserSummaryViewModel> findUserById(ObjectId id)
        {
            var user = mapper.Map<User, UserSummaryViewModel>(await userRepository.findById(id));
            user.role = await roleRepositiry.findById(user.role.id);
            return user;
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
            result.accessToken = TokenHelper.CreateAccessToken(user, setting.tokenKey, setting.accessTokenDuration);
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

        public async Task<UserSummaryViewModel> currentUser(ClaimsPrincipal principal)
        {
            var id = new ObjectId(principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName).Value);
            var user = await findUserById(id);
            if (user == null) throw new UnauthorizedException("User not found when retriving from token");
            return user;
        }
    }
}

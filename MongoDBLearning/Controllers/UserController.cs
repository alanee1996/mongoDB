using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base;
using Base.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Repositories.Entities;
using Repositories.ViewModels;
using Services.IServices;
using Services.Security;

namespace MongoDBLearning.Controllers
{
    [Route("api/authenticated/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {

        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        [Route("users")]
        public Task<IEnumerable<UserSummaryViewModel>> GetUsers()
        {
            return userService.getAllUser();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("users/create")]
        public async Task<JsonResponse> create([FromBody] UserViewModel user)
        {
            try
            {
                return await userService.create(user) ? JsonResponse.success("User save successful", null) : throw new Exception("User cannot be saved due to internal error");
            }
            catch (InvalidDataException ex)
            {
                return JsonResponse.successButInvalid(ex.Message);
            }
        }

        [HttpGet]
        [Route("users/{id}")]
        public async Task<JsonResponse> get(string id)
        {
            var user = await userService.findUserById(new ObjectId(id));
            if (user == null) throw new NotFoundException($"User with id {id} not found");
            return JsonResponse.success("Request successful", user);
        }

        [HttpDelete]
        [Route("users/delete/{id}")]
        public async Task<JsonResponse> delete(string id)
        {
            try
            {
                var user = await userService.findUserById(new ObjectId(id));
                if (user == null) throw new NotFoundException($"User with id {id} not found");
                return await userService.delete(null) ? JsonResponse.success("User delete successful", null) : throw new Exception("Delete user failed due to internal process error");
            }
            catch (UserNotFoundException ex)
            {
                return JsonResponse.successButInvalid(ex.Message);
            }
            catch (InvalidDataException ex)
            {
                return JsonResponse.successButInvalid(ex.Message);
            }
        }

        [HttpPatch]
        [Route("users/update/{id}")]
        public async Task<JsonResponse> update(string id, [FromBody] UserViewModel user)
        {
            try
            {
                if (await userService.update(user, id))
                {
                    return JsonResponse.success("User update successful", await userService.findUserById(new ObjectId(id)));
                }
                else
                {
                    return JsonResponse.successButInvalid("User update failed");
                }
            }
            catch (UserNotFoundException ex)
            {
                return JsonResponse.successButInvalid(ex.Message);
            }
            catch (InvalidDataException ex)
            {
                return JsonResponse.successButInvalid(ex.Message);
            }

        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<JsonResponse> login([FromBody] UserLoginViewModel model)
        {
            try
            {
                var user = await userService.loginUser(model);
                return JsonResponse.success("Login successful", user).setTokenInfo(user.accessToken, user.refreshToken);
            }
            catch (UserNotFoundException ex)
            {
                return JsonResponse.successButInvalid(ex.Message);
            }
            catch (InvalidDataException ex)
            {
                return JsonResponse.successButInvalid(ex.Message);
            }
        }

        //this is for testing purpose
        [HttpGet]
        [Route("users/{id}/all")]
        [AllowAnonymous]
        public async Task<JsonResponse> getUserExcept(string id)
        {
            return JsonResponse.success("Request successful", await userService.getUserExceptById(id));
        }
    }
}
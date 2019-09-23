using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base;
using Base.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Entities;
using Services.IServices;

namespace MongoDBLearning.Controllers
{
    [Route("api/authenticated/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        [Route("users")]
        public Task<IEnumerable<User>> GetUsers()
        {
            return userService.getAllUser();
        }

        [HttpPost]
        [Route("users/create")]
        public async Task<JsonResponse> create([FromBody] User user)
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
            var user = await userService.findUserById(id);
            if (user == null) throw new NotFoundException($"User with id {id} not found");
            return JsonResponse.success("Request successful", user);
        }

        [HttpDelete]
        [Route("users/delete/{id}")]
        public async Task<JsonResponse> delete(string id)
        {
            var user = await userService.findUserById(id);
            if (user == null) throw new NotFoundException($"User with id {id} not found");
            return await userService.delete(user) ? JsonResponse.success("User delete successful", null) : throw new Exception("Delete user failed due to internal process error");
        }

        [HttpPatch]
        [Route("users/update/{id}")]
        public async Task<JsonResponse> update(string id, [FromBody] User user)
        {
            if (await userService.update(user, id))
            {
                return JsonResponse.success("User update successful", await userService.findUserById(id));
            }
            else
            {
                return JsonResponse.successButInvalid("User update failed");
            }
        }
    }
}
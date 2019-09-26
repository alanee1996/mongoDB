using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base;
using Base.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Implementations;
using Services.IServices;

namespace MongoDBLearning.Controllers
{
    [Route("api/authenticated/[controller]")]
    [ApiController]
    //[Authorize]
    public class RoleController : ControllerBase
    {

        private readonly IRoleService roleService;

        public RoleController(IRoleService roleService)
        {
            this.roleService = roleService;
        }

        [HttpGet]
        [Route("roles")]
        public async Task<JsonResponse> Get()
        {
            var result = await roleService.getAllRoles();
            return JsonResponse.success("Request successful", result);
        }


        [HttpGet]
        [Route("roles/{id}")]
        public async Task<JsonResponse> Get(string id)
        {
            var result = await roleService.getRoleById(id);
            if (result == null) throw new NotFoundException($"Role not found with id {id}");
            return JsonResponse.success("Request successful", result);
        }
    }
}
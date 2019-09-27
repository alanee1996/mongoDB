using AutoMapper;
using Base;
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
    public class RoleService : IRoleService
    {
        private readonly RoleRepository roleRepository;
        private readonly IMapper mapper;
        public RoleService(RoleRepository roleRepository, IMapper mapper)
        {
            this.roleRepository = roleRepository;
            this.mapper = mapper;
        }

        public async Task<bool> checkPermission(string slug, params string[] permissions)
        {
            var temp = await roleRepository.findBySlug(slug);
            var role = mapper.Map<Role, RoleViewModel>(temp);
            if (role == null) throw new Exception("Role not found during permission checking, this can due to the role is deleted or system error");
            foreach (var permission in permissions)
            {
                if (role.permissions.Count(c => c.permission.slug == permission) == 0) return false;
            }
            return true;
        }

        public async Task<IEnumerable<RoleViewModel>> getAllRoles()
        {
            var temp = await roleRepository.findAll();
            var roles = temp.ToList();
            int count = roles.Count();
            var result = mapper.Map<List<Role>, List<RoleViewModel>>(roles);
            return result;
        }

        public async Task<RoleViewModel> getRoleById(string id)
        {
            var result = await roleRepository.findById(new ObjectId(id));
            return mapper.Map<Role, RoleViewModel>(result);
        }

        public async Task<PageResult<RoleViewModel>> getRolesByPage(int page)
        {
            var result = roleRepository.findAllInPage(page);
            return mapper.Map<PageResult<Role>, PageResult<RoleViewModel>>(await result);
        }
    }
}

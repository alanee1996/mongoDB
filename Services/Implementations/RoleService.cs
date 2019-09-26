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

        public async Task<IEnumerable<RoleViewModel>> getAllRoles()
        {
            return mapper.Map<IEnumerable<Role>, IEnumerable<RoleViewModel>>(await roleRepository.findAll());
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

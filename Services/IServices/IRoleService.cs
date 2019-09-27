using Base;
using Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleViewModel>> getAllRoles();
        Task<PageResult<RoleViewModel>> getRolesByPage(int page);
        Task<RoleViewModel> getRoleById(string id);
        Task<bool> checkPermission(string slug);
    }
}

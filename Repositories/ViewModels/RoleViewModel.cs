using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.ViewModels
{
    public class RoleViewModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public bool isActive { get; set; }
        public IEnumerable<RolePermissionViewModel> permissions { get; set; }

        public RoleViewModel()
        {
            permissions = new List<RolePermissionViewModel>();
        }
    }

    public class RolePermissionViewModel
    {
        public string id { get; set; }
        public string roleId { get; set; }
        public string userId { get; set; }
        public string permissionId { get; set; }
        public RoleViewModel role { get; set; }
        public UserViewModel user { get; set; }
        public Permission permission { get; set; }
    }

}

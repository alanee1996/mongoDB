using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Repositories.ViewModels
{
    public class UserViewModel
    {
        public string id { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public DateTime dob { get; set; }
        public string password { get; set; }
        public bool isActive { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string roleId { get; set; }

        public UserViewModel()
        {
            this.createdAt = DateTime.Now;
            this.updatedAt = DateTime.Now;
        }
    }

    public class UserSummaryViewModel
    {
        public string id { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public DateTime dob { get; set; }
        public bool isActive { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public Role role { get; set; }
        public string refreshToken { get; set; }
        public string accessToken { get; set; }
    }

    public class UserLoginViewModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public bool remember { get; set; }
    }

    public class UserIdentity : IPrincipal
    {
        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            return this.role.name == role;
        }

        public UserIdentity(string id)
        {
            this.id = id;
            Identity = new GenericIdentity(id);
        }

        public string id { get; private set; }
        public string username { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public DateTime dob { get; set; }
        public bool isActive { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public Role role { get; set; }
    }

}
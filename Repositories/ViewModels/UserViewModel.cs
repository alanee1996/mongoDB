using Repositories.Entities;
using System;
using System.Collections.Generic;
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
        public bool rememberMe { get; set; }
    }

}
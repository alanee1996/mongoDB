using AutoMapper;
using Base;
using Repositories.Entities;
using Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<User, UserViewModel>().ForMember("id", c => c.Ignore()).BeforeMap((u, um) =>
            {
                um.id = u.id.ToString();
                um.role = u.roles != null || u.roles.Count() > 0 ? u.roles.FirstOrDefault() : null;
            });
            CreateMap<PageResult<User>, PageResult<UserViewModel>>();
            CreateMap<UserViewModel, User>();
        }
    }
}

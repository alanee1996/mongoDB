using AutoMapper;
using Base;
using MongoDB.Bson;
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
                um.roleId = u.roles != null || u.roles.Count() > 0 ? u.roles.FirstOrDefault().id.ToString() : null;
            });
            CreateMap<User, UserSummaryViewModel>().ForMember("id", c => c.Ignore()).BeforeMap((u, um) =>
            {
                um.id = u.id.ToString();
                um.role = u.roles != null || u.roles.Count() > 0 ? u.roles.FirstOrDefault() : null;
            });
            CreateMap<PageResult<User>, PageResult<UserSummaryViewModel>>();
            CreateMap<UserViewModel, User>().ForMember("id", c => c.Ignore()).ForMember("roleId", c => c.Ignore()).
                BeforeMap((um, u) =>
                {
                    u.roleId = new ObjectId(um.roleId);
                }
            );

            CreateMap<Role, RoleViewModel>().ForMember("id", c => c.Ignore()).BeforeMap((r, rm) => rm.id = r.id.ToString());
            CreateMap<RoleViewModel, Role>().ForMember("id", c => c.Ignore()).BeforeMap((rm, r) => r.id = new ObjectId(rm.id));
            CreateMap<PageResult<Role>, PageResult<RoleViewModel>>();
        }
    }
}

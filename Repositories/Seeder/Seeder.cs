using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Repositories.Seeder
{
    public class Seeder : ISeeder
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task Run(DBEntities context, RepoCollections repos)
        {
            //await RunPermissionSeeder(context, repos);
            //await RunRoleSeeder(context, repos);
            //await RunUserSeeder(context, repos);
            //await RunRolePermissionSeeder(context, repos);
        }

        private async Task RunPermissionSeeder(DBEntities context, RepoCollections repos)
        {
            if (context.getCollection<Permission>().Count(c => c.isActive) == 0)
            {
                await context.RunSeeder(new List<Permission> {
                new Permission {
                    slug = "VIEW_ROLE",
                    name = "View Role",
                    isActive = true
                },
                new Permission {
                    slug = "CREATE_ROLE",
                    name = "Create Role",
                    isActive = true
                },
                new Permission {
                    slug = "EDIT_ROLE",
                    name = "Edit Role",
                    isActive = true
                },
                new Permission {
                    slug = "DELETE_ROLE",
                    name = "Delete Role",
                    isActive = true
                }
            });
            }
        }

        private async Task RunRoleSeeder(DBEntities context, RepoCollections repos)
        {
            var roles = await repos.roleRepository.findAll();
            if (roles.Count() == 0)
            {
                await context.RunSeeder(new List<Role> {
                 new Role{
                     name = "Super Admin",
                     slug = "SUPER_ADMIN",
                     isActive = true,
                 },
                 new Role{
                     name = "Admin",
                     slug = "ADMIN",
                     isActive = true,
                 }
            });
            }

        }

        private async Task RunUserSeeder(DBEntities context, RepoCollections repos)
        {
            var users = await repos.userRepository.findAll();
            if (users.Count() == 0)
            {
                var roles = await repos.roleRepository.findAll();
                var role = roles.FirstOrDefault();
                if (role != null)
                {
                    await context.RunSeeder(new List<User>{
                new User
                {
                    username = "alanee1996",
                    email = "alan@example.com",
                    dob = DateTime.Now,
                    isActive = true,
                    name = "ee",
                    roleId = role.id,
                    createdAt = DateTime.Now,
                    updatedAt = DateTime.Now,
                    password = BCrypt.Net.BCrypt.HashPassword("123123")
                }
            });
                }
            }

        }

        private async Task RunRolePermissionSeeder(DBEntities context, RepoCollections repos)
        {
            var users = await repos.userRepository.findAll();
            var user = users.FirstOrDefault();
            var role = user.roles.FirstOrDefault();
            var permissions = await context.getCollection<Permission>().FindAsync(c => c.isActive);
            await context.RunSeeder(permissions.ToList().Select(c => new RolePermission {
                roleId = role.id,
                permissionId = c.id,
                userId = user.id,
            }).ToList());
        }
    }
}

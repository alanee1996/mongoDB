using Repositories.ARepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Seeder
{
    public class RepoCollections
    {
        public readonly UserRepository userRepository;
        public readonly RoleRepository roleRepository;

        public RepoCollections(UserRepository userRepository, RoleRepository roleRepository)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
        }
    }
}

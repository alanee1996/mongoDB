using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Seeder
{
    public interface ISeeder : IDisposable
    {
        Task Run(DBEntities context, RepoCollections repos);
    }
}

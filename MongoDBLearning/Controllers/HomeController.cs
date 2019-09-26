using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Seeder;

namespace MongoDBLearning.Controllers
{
    [Controller]
    public class HomeController : Controller
    {
        //private DBEntities db;
        //private RepoCollections collections;

        //public HomeController(DBEntities db, RepoCollections collections)
        //{
        //    this.db = db;
        //    this.collections = collections;
        //}
        public async Task<IActionResult> Index()
        {
            //await new Seeder().Run(db, collections);
            return View();
        }
    }
}
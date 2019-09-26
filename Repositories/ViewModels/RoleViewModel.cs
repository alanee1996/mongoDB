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
    }
}

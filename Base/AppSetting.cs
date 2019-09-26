using System;
using System.Collections.Generic;
using System.Text;

namespace Base
{
    public class AppSetting
    {
        public string connectionString { get; set; }
        public string dbname { get; set; }
        public int pageSize { get; set; }
        public string tokenKey { get; set; }
        public int refeshTokenDuration { get; set; }
        public int accessTokenDuration { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.ViewModels
{
    public class ChatModel
    {
        public string username { get; set; }
        public DateTime datetime { get; set; }
        public string text { get; set; }
        public bool isOwn { get; set; }
    }
}

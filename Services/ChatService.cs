using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatService : Hub
    {
        public async Task sendToAll(ChatModel model)
        {
            await Clients.All.SendCoreAsync("ReceiveMessage", new object[] { model });
        }
    }
}

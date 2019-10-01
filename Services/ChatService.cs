using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Services
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatService : Hub
    {
        private IUserService userService;
        public static Dictionary<string, string> users = new Dictionary<string, string>();

        public ChatService(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task sendToAll(ChatModel model)
        {
            var currentUserId = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var currentUsername = await userService.getUsernameById(currentUserId);
            userService.Dispose();
            model.username = currentUsername;
            model.datetime = DateTime.Now;
            await Clients.AllExcept(Context.ConnectionId).SendCoreAsync("ReceiveMessage", new object[] { model });
        }

        public async Task sendPrivate(ChatModel model, string toUserId)
        {
            var currentUserId = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var currentUsername = await userService.getUsernameById(currentUserId);
            userService.Dispose();
            model.username = currentUsername;
            model.datetime = DateTime.Now;
            string targetId = "";
            lock (users)
            {
                if (users.ContainsKey(toUserId))
                {
                    targetId = users.FirstOrDefault(c => c.Key == toUserId).Value;
                }
            }
            await Clients.Client(targetId).SendCoreAsync("ReceiveMessage", new object[] { model });

        }

        public override Task OnConnectedAsync()
        {
            var userid = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var connectionId = Context.ConnectionId;
            lock (users)
            {
                if (users.Count(c => c.Key == userid) > 0)
                {
                    users.Remove(userid);
                }
                users.Add(userid, connectionId);
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            lock (users)
            {
                var user = users.FirstOrDefault(c => c.Value == connectionId);
                if(user.Value != null)
                {
                    users.Remove(user.Key);
                }
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}

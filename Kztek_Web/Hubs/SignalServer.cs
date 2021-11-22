
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kztek_Web.Hubs
{
    public class SignalServer:Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("refreshEmployees", user, message);
        }
    }
}

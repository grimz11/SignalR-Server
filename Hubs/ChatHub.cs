using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SignalRServer
{
    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"Connection id: {Context.ConnectionId}");
            Clients.Client(Context.ConnectionId).SendAsync("ReceivedConnectionId", Context.ConnectionId);
            
            return base.OnConnectedAsync();
        }
        public async Task SendMessageAsync(string message)
        {
            Console.WriteLine($"Message Received on: {Context.ConnectionId}");
            var routeObject = JsonConvert.DeserializeObject<dynamic>(message);
            Console.WriteLine($"route", routeObject);
            string toClient = routeObject.To;
            
            if(toClient == string.Empty)
            {
                await Clients.All.SendAsync("RecieveMessageBroadcast", message);
            }
            else 
            {
                await Clients.Client(toClient).SendAsync("RecieveMessageSingle", message);
            }
        }
    }
}
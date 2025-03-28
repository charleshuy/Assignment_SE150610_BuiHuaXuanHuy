using Microsoft.AspNetCore.SignalR;

namespace PharmaceuticalManagement_BuiHuaXuanHuy.Hubs
{
    public class CrudHub : Hub
    {
        public async Task SendDeleteNotification(string id)
        {
            await Clients.All.SendAsync("ReceiveDeleteNotification", id);
        }
    }
}

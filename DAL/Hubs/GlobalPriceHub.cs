using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace DAL.Hubs
{
    public class GlobalPriceHub : Hub
    {
        public static void ShowGlobalPrices()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<GlobalPriceHub>();
            context.Clients.All.loadGlobalPrices();
        }
    }
}
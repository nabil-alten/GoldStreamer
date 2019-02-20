using System.Configuration;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace DAL.Repositories
{
    [HubName("priceViewerHub")]
    public class PricesHub : Hub
    {
        private static string conString = ConfigurationManager.ConnectionStrings["GoldStreamerContext"].ToString();
        [HubMethodName("sendPrices")]
        public static void SendPrices()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<PricesHub>();
            context.Clients.All.updatePrices();
        }
    }
}
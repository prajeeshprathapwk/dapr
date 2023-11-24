using Microsoft.AspNetCore.Mvc.RazorPages;
using WarehouseManagement.UI.Models;
using DaprClient = Dapr.Client.DaprClient;

namespace WarehouseManagement.UI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly DaprClient dapr;

        public IndexModel(ILogger<IndexModel> logger, DaprClient dapr)
        {
            _logger = logger;
            this.dapr = dapr;
        }

        public async Task OnGet()
        {
            //Invoke API directly
            //var httpClient = DaprClient.CreateInvokeHttpClient();
            //var orders = await httpClient.GetFromJsonAsync<IList<ProductOrder>>("http://warehousemanagement-orders/orders");
            //Invoke side car
            var httpClient = DaprClient.CreateInvokeHttpClient("warehousemanagement-orders");
            var orders = await httpClient.GetFromJsonAsync<IList<ProductOrder>>("orders");
            //var orders = await dapr.InvokeMethodAsync<IList<ProductOrder>>(HttpMethod.Get, "warehousemanagement-orders", "orders");
            ViewData["OrderData"] = orders;
        }
    }
}
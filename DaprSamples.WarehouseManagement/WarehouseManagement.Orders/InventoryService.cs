using Dapr.Client;
using WarehouseManagement.Core;
using WarehouseManagement.Orders.Models;

namespace WarehouseManagement.Orders
{
    public class InventoryService : IInventoryService
    {
        private readonly DaprClient _dapr;
        private readonly ILogger<InventoryService> _logger;
        private readonly string STORE_NAME = "datastore";

        public InventoryService(DaprClient dapr, ILogger<InventoryService> logger)
        {
            _dapr = dapr;
            _logger = logger;
        }
        public async Task AddAsync(Inventory inventory, CancellationToken cancellationToken)
        {
            var existingItem = await GetByIdAsync(inventory.ProductId, cancellationToken);
            if (existingItem == null)
            {
                await _dapr.SaveStateAsync<Inventory>(STORE_NAME, inventory.ProductId.ToString(), inventory, cancellationToken: cancellationToken);
            }
        }

        public async Task<Inventory> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dapr.GetStateAsync<Inventory>(STORE_NAME, id.ToString(), cancellationToken: cancellationToken);
        }
    }
}

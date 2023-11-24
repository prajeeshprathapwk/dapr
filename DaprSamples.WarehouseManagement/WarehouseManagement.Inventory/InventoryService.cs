using Dapr.Client;
using Newtonsoft.Json;
using WarehouseManagement.Core;

namespace WarehouseManagement.Inventory
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

        public async Task AddAsync(Models.Inventory inventory, CancellationToken cancellationToken)
        {
            var existingInventory = await GetByIdAsync(inventory.ProductId, cancellationToken);
            if (existingInventory == null)
            {
                await _dapr.SaveStateAsync<Models.Inventory>(STORE_NAME, inventory.ProductId.ToString(), inventory, cancellationToken: cancellationToken);
            }
        }

        public async Task<Models.Inventory> GetByIdAsync(Guid productId, CancellationToken cancellationToken)
        {
            return await _dapr.GetStateAsync<Models.Inventory>(STORE_NAME, productId.ToString(), cancellationToken: cancellationToken);
        }

        public async Task<Models.Inventory> UpdateAsync(Guid productId, int quantity, CancellationToken cancellationToken)
        {
            var existingInventory = await GetByIdAsync(productId, cancellationToken);
            if (existingInventory != null)
            {
                existingInventory.Quantity = existingInventory.Quantity - quantity;
                await _dapr.SaveStateAsync<Models.Inventory>(STORE_NAME, productId.ToString(), existingInventory, cancellationToken: cancellationToken);
            }
            return existingInventory;
        }
    }
}

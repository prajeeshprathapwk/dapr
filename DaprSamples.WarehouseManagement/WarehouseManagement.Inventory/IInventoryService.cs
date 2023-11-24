using System.Threading;

namespace WarehouseManagement.Inventory
{
    public interface IInventoryService
    {
        Task AddAsync(Models.Inventory inventory, CancellationToken cancellationToken);
        Task<Models.Inventory> UpdateAsync(Guid productId, int quantity, CancellationToken cancellationToken);
        Task<Models.Inventory> GetByIdAsync(Guid productId, CancellationToken cancellationToken);
    }
}
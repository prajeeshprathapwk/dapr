using System.Threading;
using WarehouseManagement.Orders.Models;

namespace WarehouseManagement.Orders
{
    public interface IInventoryService
    {
        Task AddAsync(Inventory inventory, CancellationToken cancellationToken);
        Task<Inventory> GetByIdAsync(Guid productId, CancellationToken cancellationToken);
    }
}
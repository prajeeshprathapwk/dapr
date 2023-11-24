using System.Threading;
using WarehouseManagement.Orders.Models;

namespace WarehouseManagement.Orders
{
    public interface IOrderService
    {
        Task AddAsync(Order order, CancellationToken cancellationToken);
        Task CancelOrderAsync(Guid orderId, CancellationToken cancellationToken);
        Task ProcessOrderAsync(Guid orderId, CancellationToken cancellationToken);
        Task<IList<Order>> GetAllAsync(CancellationToken cancellationToken);
    }
}
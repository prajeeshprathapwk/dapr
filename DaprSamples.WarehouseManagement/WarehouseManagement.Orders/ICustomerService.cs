using System.Threading;
using WarehouseManagement.Orders.Models;

namespace WarehouseManagement.Orders
{
    public interface ICustomerService
    {
        Task AddAsync(Customer customer, CancellationToken cancellationToken);
        Task<Customer> GetByIdAsync(Guid customerId, CancellationToken cancellationToken);
    }
}
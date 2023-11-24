using System.Threading;
using WarehouseManagement.Customers.Models;

namespace WarehouseManagement.Customers
{
    public interface ICustomerService
    {
        Task AddAsync(Customer customer, CancellationToken cancellationToken);
        Task<Customer> GetByIdAsync(Guid customerId, CancellationToken cancellationToken);
    }
}
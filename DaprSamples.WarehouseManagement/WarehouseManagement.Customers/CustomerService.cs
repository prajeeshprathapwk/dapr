using Dapr.Client;
using WarehouseManagement.Customers.Models;

namespace WarehouseManagement.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly DaprClient _dapr;
        private readonly ILogger<CustomerService> _logger;
        private readonly string STORE_NAME = "datastore";

        public CustomerService(DaprClient dapr, ILogger<CustomerService> logger)
        {
            _dapr = dapr;
            _logger = logger;
        }
        public async Task AddAsync(Customer customer, CancellationToken cancellationToken)
        {
            var existingCustomer = await GetByIdAsync(customer.CustomerId, cancellationToken);
            if (existingCustomer == null)
            {
                await _dapr.SaveStateAsync<Customer>(STORE_NAME, customer.CustomerId.ToString(), customer, cancellationToken: cancellationToken);
            }
        }

        public async Task<Customer> GetByIdAsync(Guid customerId, CancellationToken cancellationToken)
        {
            return await _dapr.GetStateAsync<Customer>(STORE_NAME, customerId.ToString(), cancellationToken: cancellationToken);
        }
    }
}

using Dapr.Client;
using WarehouseManagement.Core;
using WarehouseManagement.Orders.Models;

namespace WarehouseManagement.Orders
{
    public class OrderService : IOrderService
    {
        private readonly DaprClient _dapr;
        private readonly ILogger<OrderService> _logger;
        private readonly string STORE_NAME = "datastore";

        public OrderService(DaprClient dapr, ILogger<OrderService> logger)
        {
            _dapr = dapr;
            _logger = logger;
        }

        public async Task AddAsync(Order order, CancellationToken cancellationToken)
        {
            var existingOrder = await GetByIdAsync(order.OrderId, cancellationToken);
            if (existingOrder == null)
            {
                await _dapr.SaveStateAsync<Order>(STORE_NAME, order.OrderId.ToString(), order, cancellationToken: cancellationToken);
                await AddToOrdersStateAsync(order.OrderId, cancellationToken);
            }
        }

        public async Task CancelOrderAsync(Guid orderId, CancellationToken cancellationToken)
        {
            var existingOrder = await GetByIdAsync(orderId, cancellationToken);
            if (existingOrder != null)
            {
                existingOrder.State = OrderState.Cancelled;
                await _dapr.SaveStateAsync<Order>(STORE_NAME, orderId.ToString(), existingOrder, cancellationToken: cancellationToken);
            }
        }

        public async Task ProcessOrderAsync(Guid orderId, CancellationToken cancellationToken)
        {
            var existingOrder = await GetByIdAsync(orderId, cancellationToken);
            if (existingOrder != null)
            {
                existingOrder.State = OrderState.Shipped;
                await _dapr.SaveStateAsync<Order>(STORE_NAME, orderId.ToString(), existingOrder, cancellationToken: cancellationToken);
            }
        }
        
        public async Task<IList<Order>> GetAllAsync(CancellationToken cancellationToken)
        {
            var orderIds = await _dapr.GetStateAsync<List<Guid>>(STORE_NAME, Constants.ORDER_STATE_KEY, cancellationToken: cancellationToken);
            if (orderIds == default(List<Guid>))
            {
                return new List<Order>();
            }

            var orders = new List<Order>();
            foreach (var orderId in orderIds)
            {
                var order = await GetByIdAsync(orderId, cancellationToken);
                if(order != default(Order))
                {
                    orders.Add(order);
                }
            }
            return orders;
        }

        private async Task<Order> GetByIdAsync(Guid orderId, CancellationToken cancellationToken)
        {
            return await _dapr.GetStateAsync<Order>(STORE_NAME, orderId.ToString(), cancellationToken: cancellationToken);
        }

        private async Task AddToOrdersStateAsync(Guid orderId, CancellationToken cancellationToken)
        {
            var orderIds = await _dapr.GetStateAsync<List<Guid>>(STORE_NAME, Constants.ORDER_STATE_KEY, cancellationToken: cancellationToken);
            if(orderIds == default(List<Guid>)){
                orderIds = new List<Guid>();
            }
            orderIds.Add(orderId);
            await _dapr.SaveStateAsync<List<Guid>>(STORE_NAME, Constants.ORDER_STATE_KEY, orderIds, cancellationToken: cancellationToken);
        }
    }
}

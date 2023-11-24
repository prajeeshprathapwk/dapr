using AutoMapper;
using MediatR;
using WarehouseManagement.Core;
using WarehouseManagement.Orders.Models;

namespace WarehouseManagement.Orders.Queries;

public class GetAllOrdersQuery : IRequest<IList<ProductOrder>>
{
}

public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, IList<ProductOrder>>
{
    private readonly ILogger<GetAllOrdersQueryHandler> logger;
    private readonly IOrderService orderService;
    private readonly IInventoryService inventoryService;
    private readonly IMapper mapper;

    public GetAllOrdersQueryHandler(ILogger<GetAllOrdersQueryHandler> logger, IOrderService orderService, IInventoryService inventoryService, IMapper mapper)
    {
        this.logger = logger;
        this.orderService = orderService;
        this.inventoryService = inventoryService;
        this.mapper = mapper;
    }

    public async Task<IList<ProductOrder>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var result = new List<ProductOrder>();
        var orders = await orderService.GetAllAsync(cancellationToken);
        logger.LogQuery<GetAllOrdersQuery, IList<Order>>(orders);

        foreach (var order in orders)
        {
            var inventory = await inventoryService.GetByIdAsync(order.ProductId, cancellationToken);
            var productOrder = mapper.Map<ProductOrder>(new Tuple<Order, Inventory>(order, inventory));
            result.Add(productOrder);
        }
        return result;
    }
}

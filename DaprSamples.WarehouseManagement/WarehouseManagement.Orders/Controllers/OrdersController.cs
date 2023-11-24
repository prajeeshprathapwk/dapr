using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Orders.Commands;
using WarehouseManagement.Orders.Models;
using WarehouseManagement.Orders.Queries;

namespace WarehouseManagement.Orders.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> logger;
        private readonly IMediator mediator;
        private readonly IValidator<Order> validator;
        private readonly IMapper mapper;

        public OrdersController(ILogger<OrdersController> logger,
            IMediator mediator,
            IValidator<Order> validator,
            IMapper mapper)
        {
            this.logger = logger;
            this.mediator = mediator;
            this.validator = validator;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Order order)
        {
            var result = await validator.ValidateAsync(order);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var command = mapper.Map<CreateOrderCommand>(order);
            var newOrder = await mediator.Send(command);
            return Created($"/orders/{order.OrderId}", newOrder);
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var orders = await mediator.Send(new GetAllOrdersQuery());
            if (orders == default(IList<ProductOrder>))
            {
                return NotFound();
            }
            return Ok(orders);
        }
    }
}

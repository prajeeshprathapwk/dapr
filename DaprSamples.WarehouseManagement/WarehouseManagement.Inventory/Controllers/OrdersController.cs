using AutoMapper;
using Dapr;
using Dapr.Client;
using EnsureThat;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Core;
using WarehouseManagement.Inventory.Commands;
using WarehouseManagement.Inventory.ExternalEvents;

namespace WarehouseManagement.Orders.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private const string PUBSUB_NAME = "pubsub";
        private const string TOPIC_NAME = "orders";

        private readonly ILogger<OrdersController> logger;
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IValidator<OrderShippedEvent> eventValidator;
        private readonly IValidator<UpdateInventoryCommand> commandValidator;

        public OrdersController(ILogger<OrdersController> logger, 
            IMediator mediator, 
            IMapper mapper, 
            IValidator<OrderShippedEvent> eventValidator, 
            IValidator<UpdateInventoryCommand> commandValidator)
        {
            this.logger = logger;
            this.mediator = mediator;
            this.mapper = mapper;
            this.eventValidator = eventValidator;
            this.commandValidator = commandValidator;
        }

        [HttpPost("orders")]
        [Topic(PUBSUB_NAME, TOPIC_NAME, "event.data.type == \"OrderShippedEvent\"", 1)]
        public async Task<IActionResult> Subscribe(IntegrationEvent<OrderShippedEvent> @event, [FromServices] DaprClient daprClient)
        {
            Ensure.That(@event).IsNotNull();
            logger.LogEventSubscribed(@event);

            var eventValidationResult = await eventValidator.ValidateAsync(@event.Payload);
            if(!eventValidationResult.IsValid)
            {
                eventValidationResult.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var command = mapper.Map<UpdateInventoryCommand>(@event.Payload);

            var commandValidationResult = await commandValidator.ValidateAsync(command);
            if (!commandValidationResult.IsValid)
            {
                commandValidationResult.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var customer = await mediator.Send(command);
            return Ok(customer);
        }
    }
}
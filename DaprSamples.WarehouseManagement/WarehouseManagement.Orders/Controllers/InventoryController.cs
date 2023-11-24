using AutoMapper;
using Dapr;
using Dapr.Client;
using EnsureThat;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using WarehouseManagement.Core;
using WarehouseManagement.Orders.Commands;
using WarehouseManagement.Orders.ExternalEvents;

namespace WarehouseManagement.Orders.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private const string PUBSUB_NAME = "pubsub";
        private const string TOPIC_NAME = "inventories";

        private readonly ILogger<InventoryController> logger;
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IValidator<InventoryUpdatedEvent> eventValidator;
        private readonly IValidator<UpdateInventoryCommand> commandValidator;

        public InventoryController(ILogger<InventoryController> logger, 
            IMediator mediator, 
            IMapper mapper, 
            IValidator<InventoryUpdatedEvent> eventValidator, 
            IValidator<UpdateInventoryCommand> commandValidator)
        {
            this.logger = logger;
            this.mediator = mediator;
            this.mapper = mapper;
            this.eventValidator = eventValidator;
            this.commandValidator = commandValidator;
        }

        [HttpPost("inventory")]
        [Topic(PUBSUB_NAME, TOPIC_NAME, "event.data.type == \"InventoryUpdatedEvent\"", 2)]
        public async Task<IActionResult> Subscribe(IntegrationEvent<InventoryUpdatedEvent> @event, [FromServices] DaprClient daprClient)
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
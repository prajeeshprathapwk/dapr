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
    public class CustomerController : ControllerBase
    {
        private const string PUBSUB_NAME = "pubsub";
        private const string TOPIC_NAME = "customers";

        private readonly ILogger<CustomerController> logger;
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IValidator<CustomerCreatedEvent> eventValidator;
        private readonly IValidator<CreateCustomerCommand> commandValidator;

        public CustomerController(ILogger<CustomerController> logger, 
            IMediator mediator, 
            IMapper mapper, 
            IValidator<CustomerCreatedEvent> eventValidator, 
            IValidator<CreateCustomerCommand> commandValidator)
        {
            this.logger = logger;
            this.mediator = mediator;
            this.mapper = mapper;
            this.eventValidator = eventValidator;
            this.commandValidator = commandValidator;
        }

        [HttpPost("customers")]
        [Topic(PUBSUB_NAME, TOPIC_NAME, "event.data.type == \"CustomerCreatedEvent\"", 1)]
        public async Task<IActionResult> Subscribe(IntegrationEvent<CustomerCreatedEvent> @event, [FromServices] DaprClient daprClient)
        {
            Ensure.That(@event).IsNotNull();
            logger.LogEventSubscribed(@event);

            var eventValidationResult = await eventValidator.ValidateAsync(@event.Payload);
            if (!eventValidationResult.IsValid)
            {
                eventValidationResult.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var command = mapper.Map<CreateCustomerCommand>(@event.Payload);

            var commandValidationResult = await commandValidator.ValidateAsync(command);
            if (!commandValidationResult.IsValid)
            {
                commandValidationResult.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var customer = await mediator.Send(command);
            return Ok(customer);
        }

        //[HttpPost("customers")]
        //[Topic(PUBSUB_NAME, TOPIC_NAME)]
        //public IActionResult Subscribe(object @event, [FromServices] DaprClient daprClient)
        //{
        //    Ensure.That(@event).IsNotNull();
        //    //logger.LogEventSubscribed(@event);

        //    //var eventValidationResult = await eventValidator.ValidateAsync(@event.Payload);
        //    //if (!eventValidationResult.IsValid)
        //    //{
        //    //    eventValidationResult.AddToModelState(ModelState);
        //    //    return BadRequest(ModelState);
        //    //}

        //    //var command = mapper.Map<CreateCustomerCommand>(@event.Payload);

        //    //var commandValidationResult = await commandValidator.ValidateAsync(command);
        //    //if (!commandValidationResult.IsValid)
        //    //{
        //    //    commandValidationResult.AddToModelState(ModelState);
        //    //    return BadRequest(ModelState);
        //    //}

        //    //var customer = await mediator.Send(command);
        //    //return Ok(customer);
        //    return Ok();
        //}
    }
}
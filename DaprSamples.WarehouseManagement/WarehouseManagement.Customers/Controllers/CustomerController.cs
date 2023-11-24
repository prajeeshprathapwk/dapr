using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Customers.Commands;
using WarehouseManagement.Customers.Models;

namespace WarehouseManagement.Customers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> logger;
        private readonly IMediator mediator;
        private readonly IValidator<Customer> validator;
        private readonly IMapper mapper;

        public CustomerController(ILogger<CustomerController> logger, IMediator mediator, IValidator<Customer> validator, IMapper mapper)
        {
            this.logger = logger;
            this.mediator = mediator;
            this.validator = validator;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Customer customer)
        {
            var result = await validator.ValidateAsync(customer);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var command = mapper.Map<CreateCustomerCommand>(customer);
            var createdCustomer = await mediator.Send(command);
            return Created($"/customer/{customer.CustomerId}", createdCustomer);
        }
    }
}
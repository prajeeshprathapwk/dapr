using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Inventory.Commands;

namespace WarehouseManagement.Inventory.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<InventoryController> logger;
        private readonly IMediator mediator;
        private readonly IValidator<Models.Inventory> validator;
        private readonly IMapper mapper;

        public InventoryController(ILogger<InventoryController> logger, 
            IMediator mediator, 
            IValidator<Models.Inventory> validator, 
            IMapper mapper)
        {
            this.logger = logger;
            this.mediator = mediator;
            this.validator = validator;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Models.Inventory inventory)
        {
            var result = await validator.ValidateAsync(inventory);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }
            var command = mapper.Map<AddInventoryCommand>(inventory);
            var createdInventory = await mediator.Send(command);

            return Created($"/inventory/{inventory.ProductId}", createdInventory);
        }
    }
}
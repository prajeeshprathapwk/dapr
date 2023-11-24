using MediatR;
using WarehouseManagement.Core;
using WarehouseManagement.Orders.Models;

namespace WarehouseManagement.Orders.Commands
{
    public class CreateCustomerCommand : IRequest<Customer>
    {
        public Customer Customer { get; set; }
    }

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Customer>
    {
        private readonly ILogger<CreateCustomerCommandHandler> logger;
        private readonly ICustomerService customerService;

        public CreateCustomerCommandHandler(ILogger<CreateCustomerCommandHandler> logger, ICustomerService customerService)
        {
            this.logger = logger;
            this.customerService = customerService;
        }

        public async Task<Customer> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            await customerService.AddAsync(request.Customer, cancellationToken);
            logger.LogCommand(request);
            return request.Customer;
        }
    }
}

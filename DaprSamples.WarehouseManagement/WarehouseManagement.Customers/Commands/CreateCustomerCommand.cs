using AutoMapper;
using EnsureThat;
using MediatR;
using WarehouseManagement.Core;
using WarehouseManagement.Customers.Events;
using WarehouseManagement.Customers.Models;

namespace WarehouseManagement.Customers.Commands
{
    public class CreateCustomerCommand : IRequest<Customer>
    {
        public CreateCustomerCommand(Customer customer)
        {
            Ensure.That(customer).IsNotNull();
            Customer = customer;
        }

        public Customer Customer { get; }
    }

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Customer>
    {
        private readonly ILogger<CreateCustomerCommandHandler> logger;
        private readonly ICustomerService customerService;
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public CreateCustomerCommandHandler(ILogger<CreateCustomerCommandHandler> logger, ICustomerService customerService, IMediator mediator, IMapper mapper)
        {
            this.logger = logger;
            this.customerService = customerService;
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task<Customer> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            await customerService.AddAsync(request.Customer, cancellationToken);
            logger.LogCommand<CreateCustomerCommand>(request);

            var @event = mapper.Map<CustomerCreatedEvent>(request);
            await mediator.Publish<CustomerCreatedEvent>(@event, cancellationToken);
            return request.Customer;
        }
    }
}

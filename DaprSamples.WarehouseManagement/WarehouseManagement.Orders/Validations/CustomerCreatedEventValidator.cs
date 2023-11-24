using FluentValidation;
using WarehouseManagement.Orders.ExternalEvents;
using WarehouseManagement.Orders.Validations;

namespace WarehouseManagement.Customers.Validations
{
    public class CustomerCreatedEventValidator : AbstractValidator<CustomerCreatedEvent>
    {
        public CustomerCreatedEventValidator()
        {
            RuleFor(_ => _.EventId).NotEmpty().WithMessage("External events should have an EventId");
            RuleFor(_ => _.Customer).NotNull().WithMessage("Customer should be provided");
            RuleFor(_ => _.Customer).SetValidator(new CustomerValidator());
        }
    }
}

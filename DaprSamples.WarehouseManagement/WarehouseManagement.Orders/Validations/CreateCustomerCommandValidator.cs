using FluentValidation;
using WarehouseManagement.Orders.Commands;
using WarehouseManagement.Orders.Validations;

namespace WarehouseManagement.Customers.Validations
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(_ => _.Customer).NotEmpty().WithMessage("Customer should be provided");
            RuleFor(_ => _.Customer).SetValidator(new CustomerValidator());
        }
    }
}

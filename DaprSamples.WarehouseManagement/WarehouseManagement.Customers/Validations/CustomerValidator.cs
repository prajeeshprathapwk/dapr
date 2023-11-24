using FluentValidation;
using WarehouseManagement.Customers.Models;

namespace WarehouseManagement.Customers.Validations
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(_ => _.CustomerName).NotEmpty().WithMessage("Customer name should be provided");
            RuleFor(_ => _.Address).NotNull().WithMessage("Customer Address should be provided");
            RuleFor(_ => _.Address).SetValidator(new AddressValidator());
        }
    }
}

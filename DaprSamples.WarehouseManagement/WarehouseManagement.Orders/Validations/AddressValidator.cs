using FluentValidation;
using WarehouseManagement.Orders.Models;

namespace WarehouseManagement.Orders.Validations
{
    internal class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(_ => _.StreetName).NotEmpty().WithMessage("StreetName should be provided");
            RuleFor(_ => _.HouseNumber).NotNull().Must(_ => _ > 0).WithMessage("HouseNumber must be greater than 0");
            RuleFor(_ => _.ZipCode).NotEmpty().WithMessage("ZipCode should be provided");
            RuleFor(_ => _.State).NotEmpty().WithMessage("State should be provided");
            RuleFor(_ => _.Country).NotEmpty().WithMessage("Country should be provided");
        }
    }
}
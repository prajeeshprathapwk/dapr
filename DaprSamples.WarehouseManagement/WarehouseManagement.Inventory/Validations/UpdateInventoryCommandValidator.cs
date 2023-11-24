using FluentValidation;
using WarehouseManagement.Inventory.Commands;

namespace WarehouseManagement.Inventory.Validations
{
    public class UpdateInventoryCommandValidator : AbstractValidator<UpdateInventoryCommand>
    {
        public UpdateInventoryCommandValidator()
        {
            RuleFor(_ => _.ProductId).NotEmpty().WithMessage("ProductId should be provided");
            RuleFor(_ => _.Quantity).GreaterThan(0).WithMessage("Quantity should be greater than 0");
        }
    }
}

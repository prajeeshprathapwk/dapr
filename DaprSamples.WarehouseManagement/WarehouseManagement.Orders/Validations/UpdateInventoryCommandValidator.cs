using FluentValidation;
using WarehouseManagement.Orders.Commands;
using WarehouseManagement.Orders.Validations;

namespace WarehouseManagement.Customers.Validations
{
    public class UpdateInventoryCommandValidator : AbstractValidator<UpdateInventoryCommand>
    {
        public UpdateInventoryCommandValidator()
        {
            RuleFor(_ => _.Inventory).NotEmpty().WithMessage("Inventory should be provided");
            RuleFor(_ => _.Inventory).SetValidator(new InventoryValidator());
        }
    }
}

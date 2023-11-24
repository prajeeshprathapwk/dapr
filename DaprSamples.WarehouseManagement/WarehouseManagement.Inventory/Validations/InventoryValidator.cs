using FluentValidation;

namespace WarehouseManagement.Inventory.Validations
{
    public class InventoryValidator : AbstractValidator<Models.Inventory>
    {
        public InventoryValidator()
        {
            RuleFor(_ => _.ProductId).NotEmpty().WithMessage("ProductID should be provided");
            RuleFor(_ => _.ProductName).NotEmpty().WithMessage("Product name should be provided");
            RuleFor(_ => _.Quantity).GreaterThan(0).WithMessage("Product quantity should be greater than 0");
            RuleFor(_ => _.UnitPrice).GreaterThan(0).WithMessage("Unit price should be greater than 0");
        }
    }
}

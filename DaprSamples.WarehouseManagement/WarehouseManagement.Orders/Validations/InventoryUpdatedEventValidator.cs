using FluentValidation;
using WarehouseManagement.Orders.ExternalEvents;
using WarehouseManagement.Orders.Validations;

namespace WarehouseManagement.Customers.Validations
{
    public class InventoryUpdatedEventValidator : AbstractValidator<InventoryUpdatedEvent>
    {
        public InventoryUpdatedEventValidator()
        {
            RuleFor(_ => _.EventId).NotEmpty().WithMessage("External events should have an EventId");
            RuleFor(_ => _.Inventory).NotNull().WithMessage("Inventory should be provided");
            RuleFor(_ => _.Inventory).SetValidator(new InventoryValidator());
        }
    }
}

using FluentValidation;
using WarehouseManagement.Inventory.ExternalEvents;

namespace WarehouseManagement.Inventory.Validations
{
    public class OrderShippedEventValidator : AbstractValidator<OrderShippedEvent>
    {
        public OrderShippedEventValidator()
        {
            RuleFor(_ => _.EventId).NotEmpty().WithMessage("External events should have an EventId");
            RuleFor(_ => _.Order).NotNull().WithMessage("Order should be provided");
            RuleFor(_ => _.Order).SetValidator(new OrderValidator());
        }
    }
}

using FluentValidation;
using WarehouseManagement.Orders.Models;

namespace WarehouseManagement.Orders.Validations
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(_ => _.OrderId).NotEmpty().WithMessage("OrderId should be provided");
            RuleFor(_ => _.CustomerId).NotEmpty().WithMessage("CustomerId should be provided");
            RuleFor(_ => _.ProductId).NotEmpty().WithMessage("ProductId should be provided");
            RuleFor(_ => _.Quantity).GreaterThanOrEqualTo(0).WithMessage("Product Quantity should be greater than 0");
        }
    }
}

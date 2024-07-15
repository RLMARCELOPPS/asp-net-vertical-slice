using ecommerse_api.Features.Orders.Command.UpdateOrder;
using FluentValidation;

namespace ecommerse_api.Features.Orders.Validator
{
    public class UpdateOrderValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderValidator()
        {
            RuleFor(x => x.Status).NotEmpty().WithMessage("Status is required.");
        }
    }
}

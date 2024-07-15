using ecommerse_api.Features.CartItems.Commands.UpdateCartItem;
using FluentValidation;

namespace ecommerse_api.Features.CartItems.Validator
{
    public class UpdateCartItemValidator : AbstractValidator<UpdateCartItemCommand>
    {
        public UpdateCartItemValidator()
        {
            
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("Product name is required.");
            RuleFor(x => x.Quantity).NotEmpty().WithMessage("Quantity is required");
            RuleFor(x => x.UnitPrice).NotEmpty().WithMessage("Unit price is required");

        }

    }
}

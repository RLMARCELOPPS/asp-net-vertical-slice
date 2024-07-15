using ecommerse_api.Features.CartItems.Commands.CreateCartItem;
using FluentValidation;

namespace ecommerse_api.Features.CartItems.Validator
{
    public class CreateCartItemValidator : AbstractValidator<CreateCartItemCommand>
    {
        public CreateCartItemValidator()
        {
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("Product Name is required");
            RuleFor(x => x.Quantity).NotEmpty().WithMessage("Quantity is required");
            RuleFor(x => x.UnitPrice).NotEmpty().WithMessage("Unit price is required");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User is required");


        }
    }
}

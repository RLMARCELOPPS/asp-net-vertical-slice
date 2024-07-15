using ecommerse_api.Features.Orders.Models;
using ecommerse_api.Features.Users.Dto;

namespace ecommerse_api.Features.CartItems.Dto
{
    public class CartItemOrderDto
    {

        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime DateOrdered { get; set; }

        public List<CartItemDto> CartItems { get; set; } = null!;
        public UserDto UserInfo { get; set; } = null!;
    }
}

using ecommerse_api.Features.CartItems.Dto;
using ecommerse_api.Features.CartItems.Models;
using ecommerse_api.Features.Orders.Models;

namespace ecommerse_api.Features.Orders.Dto
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime DateOrdered { get; set; }
        public Guid UserId { get; set; }
        public virtual ICollection<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();

    }
}

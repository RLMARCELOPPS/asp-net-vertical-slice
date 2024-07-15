using ecommerse_api.Features.Orders.Models;

namespace ecommerse_api.Features.CartItems.Models
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; } = null!;
    }
}

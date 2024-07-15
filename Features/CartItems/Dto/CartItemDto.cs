namespace ecommerse_api.Features.CartItems.Dto
{
    public class CartItemDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; } 
        public Guid OrderId { get; set; }
    }
}

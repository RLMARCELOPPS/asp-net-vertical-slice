namespace ecommerse_api.Features.CartItems.Dto
{
    public class UpdateCartItemDto
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

    }
}

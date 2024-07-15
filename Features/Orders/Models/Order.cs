using ecommerse_api.Features.CartItems.Models;
using ecommerse_api.Features.Users.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ecommerse_api.Features.Orders.Models
{
    
    public class Order
    {
        public Guid Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public OrderStatus Status { get; set; }
        public DateTime DateOrdered { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public Guid UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }

    public enum OrderStatus
    {
        Pending,
        Processed,
        Cancelled,
    }
}

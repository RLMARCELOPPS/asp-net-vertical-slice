using ecommerse_api.Features.Orders.Dto;
using ecommerse_api.Features.Orders.Models;

namespace ecommerse_api.Features.Users.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public virtual ICollection<OrderDto> Orders { get; set; } = new List<OrderDto>();


    }
}

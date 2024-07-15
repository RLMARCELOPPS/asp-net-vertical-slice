using AutoMapper;
using ecommerse_api.Features.CartItems.Commands.CreateCartItem;
using ecommerse_api.Features.CartItems.Commands.UpdateCartItem;
using ecommerse_api.Features.CartItems.Dto;
using ecommerse_api.Features.CartItems.Models;
using ecommerse_api.Features.Orders.Command.UpdateOrder;
using ecommerse_api.Features.Orders.Dto;
using ecommerse_api.Features.Orders.Models;
using ecommerse_api.Features.Users.Command.CreateUser;
using ecommerse_api.Features.Users.Dto;
using ecommerse_api.Features.Users.Models;

namespace ecommerse_api.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateCartItemCommand, CartItem>();
            CreateMap<CartItem, CartItemDto>(); 
            CreateMap<UpdateCartItemDto, UpdateCartItemCommand>();


           CreateMap<CreateUserCommand, User>();
           CreateMap<User, UserDto>();

            CreateMap<Order, CartItemOrderDto>();

            CreateMap<Order, OrderDto>();
        }
    }
}

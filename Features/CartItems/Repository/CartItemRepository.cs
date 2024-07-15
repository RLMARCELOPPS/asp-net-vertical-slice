using AutoMapper;
using Dapper;
using ecommerse_api.Common.Infastracture;
using ecommerse_api.Features.CartItems.Models;
using ecommerse_api.Features.Orders.Models;
using ecommerse_api.Features.Orders.Repository;
using MySql.Data.MySqlClient;

namespace ecommerse_api.Features.CartItems.Repository
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;

        public CartItemRepository(ApplicationDbContext context, IOrderRepository orderRepository, IMapper mapper) {
            _context = context;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public async Task CreateAysnc(CartItem cartItem)
        {
            await _context.AddAsync(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task<CartItem?> DeleteAysnc(Guid id)
        {
            var cartItem = await GetByIdAsync(id);
            if(cartItem == null)
            {
                return null;
            }
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<IEnumerable<CartItem>> GetAllByOrderAsync(Guid orderId)
        {
            using var connection = new MySqlConnection(ApplicationDbContext.GlobalDbConnection.MysqlConnection);
            var query = "SELECT * FROM CartItems WHERE OrderId = @OrderId";
            var carItems = await connection.QueryAsync<CartItem>(query, new { OrderId = orderId });

            return carItems.ToList();
        }

        public async Task<IEnumerable<CartItem>> GetAllPendingAsync()
        {
            using var connection = new MySqlConnection(ApplicationDbContext.GlobalDbConnection.MysqlConnection);
            var pendingOrders = await _orderRepository.GetAllStatusOrderAsync(OrderStatus.Pending.ToString());

            var pendingCartItems = new List<CartItem>();

            foreach (var order in pendingOrders)
            {

                var query = "SELECT * FROM CartItems WHERE OrderId = @OrderId";
                var cartItems = await connection.QueryAsync<CartItem>(query, new { OrderId = order.Id });

                pendingCartItems.AddRange(cartItems);
            }

            return pendingCartItems;
        }
        public async Task<CartItem?> GetByIdAsync(Guid id)
        {
            using var connection = new MySqlConnection(ApplicationDbContext.GlobalDbConnection.MysqlConnection);
            var query = "SELECT * FROM CartItems WHERE Id = @Id";
            var cartItem = await connection.QueryFirstOrDefaultAsync<CartItem>(query, new { Id = id });
            if(cartItem == null)
            {
                return null;
            }

            return cartItem;

        }

        public async Task UpdateAynsc(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
        }
    }
}

using AutoMapper;
using Dapper;
using ecommerse_api.Common.Infastracture;
using ecommerse_api.Features.Orders.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace ecommerse_api.Features.Orders.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMapper  _mapper;
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<Order>> GetAllOrderAsync()
        {
            using var connection = new MySqlConnection(ApplicationDbContext.GlobalDbConnection.MysqlConnection);
            var query = "SELECT * FROM Orders";
            var orders = await connection.QueryAsync<Order>(query);
            return orders.ToList();
        }

        public async Task<List<Order>> GetAllStatusOrderAsync(string status)
        {
            using var connection = new MySqlConnection(ApplicationDbContext.GlobalDbConnection.MysqlConnection);
            var query = "SELECT * FROM Orders WHERE Status = @Status";

            var pendingOrder = (await connection.QueryAsync<Order>(query, new { Status = status })).ToList();

            return pendingOrder;
        }

        public async Task<Order> GetByIdAsync(Guid id)
        {
            using var connection = new MySqlConnection(ApplicationDbContext.GlobalDbConnection.MysqlConnection);
            
            var query = "SELECT * FROM Orders WHERE Id = @Id";

            var order = await connection.QuerySingleOrDefaultAsync<Order>(query, new { Id = id });

            if(order == null)
            {
                throw new Exception("Order not Found.");
            }

            return order;

        }


        
        public async Task<Guid> GetOrCreatePendingOrderIdAync(Guid userId)
        {
            using var connection = new MySqlConnection(ApplicationDbContext.GlobalDbConnection.MysqlConnection);

            var query = "SELECT * FROM Orders WHERE UserId = @UserId AND Status = @Status";

            var existingOrder = await connection.QuerySingleOrDefaultAsync<Order>(query, new
            {
                UserId = userId,
                Status = OrderStatus.Pending.ToString()
            });

            if(existingOrder != null)
            {
                return existingOrder.Id;
            }
            
            var newOrder = new Order
            {
                UserId = userId,
                Status = OrderStatus.Pending
            };

            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();
            _context.Entry(newOrder).Reload();

            return newOrder.Id;

        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

        }
        public async Task DeleteAsync(Guid id)
        {
            var order = await GetByIdAsync(id);

            if(order == null)
            {
                throw new Exception("Order not Found.");
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetOrdersByUserAsync(Guid userId)
        {
            using var connection = new MySqlConnection(ApplicationDbContext.GlobalDbConnection.MysqlConnection);

            var query = "SELECT * FROM Orders WHERE UserId = @UserId";

            var order = (await connection.QueryAsync<Order>(query, new { UserId = userId })).ToList();
           
            return order;

        }
    }
}

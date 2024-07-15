using AutoMapper;
using Dapper;
using ecommerse_api.Common.Infastracture;
using ecommerse_api.Features.Users.Models;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace ecommerse_api.Features.Users.Repository
{
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateAsync(User user, CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            using var connection = new MySqlConnection(ApplicationDbContext.GlobalDbConnection.MysqlConnection);

            var query = "SELECT * FROM Users WHERE ID = @Id";

            var user = await connection.QuerySingleOrDefaultAsync<User>(query, new { Id = id });

            return _mapper.Map<User>(user);
        }

        public async Task<bool> UserExistAsync(Guid id)
        {
           return await _context.Users.AnyAsync(u => u.Id == id);
        }
        
        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _context.Users.AnyAsync( u => u.Email == email);
        }
    }
}


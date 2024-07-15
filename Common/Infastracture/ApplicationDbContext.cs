using ecommerse_api.Common.Logger;
using ecommerse_api.Features.CartItems.Models;
using ecommerse_api.Features.Orders.Models;
using ecommerse_api.Features.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace ecommerse_api.Common.Infastracture
{
    public class ApplicationDbContext : DbContext
    {
        private readonly LoggingInterceptor _loggingInterceptor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, LoggingInterceptor loggingInterceptor)
            : base(options)
        {
            _loggingInterceptor = loggingInterceptor;
        }

        public static class GlobalDbConnection
        {
            public static string MysqlConnection { get; set; } = null!;
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<CartItem> CartItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasMany(c => c.CartItems)
                .WithOne(c => c.Order)
                .HasForeignKey(c => c.OrderId);

            modelBuilder.Entity<User>()
                .HasMany(o => o.Orders)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v)
                );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_loggingInterceptor);
        }
    }
}

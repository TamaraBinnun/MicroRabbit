using MicroRabbit.Orders.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Orders.Data.Context
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>().HasMany(b => b.OrderItems).WithOne(o => o.Book!).HasForeignKey(o => o.BookId);
            modelBuilder.Entity<OrderItem>().HasOne(o => o.Book).WithMany(b => b.OrderItems).HasForeignKey(b => b.OrderId);
        }
    }
}
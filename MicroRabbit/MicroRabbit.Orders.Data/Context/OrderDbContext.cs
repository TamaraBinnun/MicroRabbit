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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>().HasMany(order => order.OrderItems).WithOne(orderItem => orderItem.Order!).HasForeignKey(orderItem => orderItem.OrderId);
            modelBuilder.Entity<OrderItem>().HasOne(orderItem => orderItem.Order).WithMany(order => order.OrderItems);

            modelBuilder.Entity<Book>().HasMany(book => book.OrderItems).WithOne(orderItem => orderItem.Book!).HasForeignKey(orderItem => orderItem.BookId);
            modelBuilder.Entity<OrderItem>().HasOne(orderItem => orderItem.Book).WithMany(book => book.OrderItems);
        }
    }
}
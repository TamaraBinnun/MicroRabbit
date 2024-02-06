using MicroRabbit.Orders.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

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

        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderItem>().HasIndex(e => new { e.OrderId, e.BookId }).IsUnique();

            modelBuilder.Entity<Order>().HasMany(order => order.OrderItems).WithOne(orderItem => orderItem.Order!).HasForeignKey(orderItem => orderItem.OrderId);
            modelBuilder.Entity<OrderItem>().HasOne(orderItem => orderItem.Order).WithMany(order => order.OrderItems);

            modelBuilder.Entity<Book>().HasMany(book => book.OrderItems).WithOne(orderItem => orderItem.Book!).HasForeignKey(orderItem => orderItem.BookId);
            modelBuilder.Entity<OrderItem>().HasOne(orderItem => orderItem.Book).WithMany(book => book.OrderItems);

            //one to one relationship
            modelBuilder.Entity<Order>().HasOne(e => e.Payment).WithOne(e => e.Order).HasForeignKey<Payment>(e => e.OrderId).IsRequired();
            modelBuilder.Entity<Payment>().HasOne(e => e.Order).WithOne(e => e.Payment).HasForeignKey<Order>(e => e.PaymentId);
        }
    }
}
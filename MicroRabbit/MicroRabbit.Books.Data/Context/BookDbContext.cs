using MicroRabbit.Books.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Books.Data.Context
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<OrderedBook> OrderedBooks { get; set; }
        public DbSet<Publication> Publications { get; set; }
    }
}
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>().HasIndex(e => e.ISBN).IsUnique();
            modelBuilder.Entity<BookCategory>().HasIndex(e => new { e.BookId, e.CategoryId }).IsUnique();

            //many-to-many relationship
            modelBuilder.Entity<Book>().HasMany(p => p.Categories).WithMany(p => p.Books)
                                       .UsingEntity<BookCategory>
                                        (
                                            j => j.HasOne(t => t.Category).WithMany(p => p.BookCategory),
                                            j => j.HasOne(t => t.Book).WithMany(p => p.BookCategory)
                                        );
        }
    }
}
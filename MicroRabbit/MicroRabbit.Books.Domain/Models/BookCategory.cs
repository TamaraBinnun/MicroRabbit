using MicroRabbit.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Books.Domain.Models
{
    [Index(nameof(BookId), nameof(CategoryId), IsUnique = true)]
    public class BookCategory : BaseModel
    {
        public int BookId { get; set; }

        public Book Book { get; set; } = null!;////one to many relationship

        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;////one to many relationship
    }
}
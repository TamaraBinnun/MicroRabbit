using System.ComponentModel.DataAnnotations;

namespace MicroRabbit.Books.Domain.Models
{
    public class BookCategory
    {
        [Key]
        public int Id { get; set; }

        public int BookId { get; set; }

        public int CategoryId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
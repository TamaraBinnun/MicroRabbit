using System.ComponentModel.DataAnnotations;

namespace MicroRabbit.Books.Domain.Models
{
    public class BookInStock
    {
        [Key]
        public int Id { get; set; }

        public int BookId { get; set; }

        public int Units { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
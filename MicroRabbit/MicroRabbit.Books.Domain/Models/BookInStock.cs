using MicroRabbit.Domain.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace MicroRabbit.Books.Domain.Models
{
    public class BookInStock : BaseModel
    {
        public int BookId { get; set; }

        public int Units { get; set; }
    }
}
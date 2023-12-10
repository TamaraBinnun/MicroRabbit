using MicroRabbit.Domain.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace MicroRabbit.Books.Domain.Models
{
    public class BookCategory : BaseModel
    {
        public int BookId { get; set; }

        public int CategoryId { get; set; }
    }
}
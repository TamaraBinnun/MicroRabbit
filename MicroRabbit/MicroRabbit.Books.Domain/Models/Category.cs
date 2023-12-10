using MicroRabbit.Domain.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace MicroRabbit.Books.Domain.Models
{
    public class Category : BaseModel
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        public int? ParentCategoryId { get; set; }
    }
}
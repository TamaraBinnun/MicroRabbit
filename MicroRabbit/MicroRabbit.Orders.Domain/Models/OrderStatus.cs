using MicroRabbit.Domain.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace MicroRabbit.Orders.Domain.Models
{
    public class OrderStatus : BaseModel
    {
        public int Status { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; } = null!;
    }
}
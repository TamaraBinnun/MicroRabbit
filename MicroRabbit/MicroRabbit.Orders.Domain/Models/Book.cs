using MicroRabbit.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MicroRabbit.Orders.Domain.Models
{
    [Index(nameof(ISBN), IsUnique = true)]
    public class Book : BaseModel
    {
        [MaxLength(20)]
        public string ISBN { get; set; } = null!; //מספר ספר תקני בין-לאומי מסת"ב International Standard Book Number

        [Required]
        [MaxLength(50)]
        public string Title { get; set; } = null!;

        public int ExternalId { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();//one to many relationship
    }
}
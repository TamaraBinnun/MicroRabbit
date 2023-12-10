using System.ComponentModel.DataAnnotations;

namespace MicroRabbit.Domain.Core.Models
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
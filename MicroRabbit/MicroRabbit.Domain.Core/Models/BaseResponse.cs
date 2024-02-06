namespace MicroRabbit.Domain.Core.Models
{
    public class BaseResponse
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
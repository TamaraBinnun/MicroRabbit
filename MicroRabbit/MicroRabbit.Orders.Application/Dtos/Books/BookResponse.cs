using MicroRabbit.Domain.Core.Models;

namespace MicroRabbit.Orders.Application.Dtos
{
    public class BookResponse : BaseResponse
    {
        public string ISBN { get; set; } = null!;

        public string Title { get; set; } = null!;

        public int ExternalId { get; set; }
    }
}
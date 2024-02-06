using MicroRabbit.Domain.Core.Models;

namespace MicroRabbit.Orders.Application.Dtos.Books
{
    public class UpdateBookRequest : UpdateBaseRequest
    {
        public string Title { get; set; } = null!;
    }
}
using MicroRabbit.Domain.Core.Interfaces;
using MicroRabbit.Domain.Core.Models;
using MicroRabbit.Orders.Domain.Models;

namespace MicroRabbit.Orders.Domain.Interfaces
{
    public interface IBooksRepository<UpdateTRequest> : IRepository<Book, UpdateTRequest>
         where UpdateTRequest : UpdateBaseRequest
    {
    }
}
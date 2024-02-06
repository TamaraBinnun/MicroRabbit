using MicroRabbit.Books.Domain.Models;
using MicroRabbit.Domain.Core.Interfaces;
using MicroRabbit.Domain.Core.Models;

namespace MicroRabbit.Books.Domain.Interfaces
{
    public interface IBooksRepository<UpdateTRequest> : IRepository<Book, UpdateTRequest>
         where UpdateTRequest : UpdateBaseRequest

    {
    }
}
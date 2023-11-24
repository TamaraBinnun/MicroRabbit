using MicroRabbit.Domain.Core.Dtos;
using MicroRabbit.Orders.Domain.Interfaces;

namespace MicroRabbit.Orders.Infrastructure.Synchronous.Services
{
    public class GrpcMicroRabbitBooksClient : IMicroRabbitBooksClient
    {
        public Task<IEnumerable<BookUnits>?> GetBookUnitsInStockAsync(List<int> bookIds)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateBookUnitsInStockAsync(List<BookUnits> bookUnits)
        {
            throw new NotImplementedException();
        }
    }
}
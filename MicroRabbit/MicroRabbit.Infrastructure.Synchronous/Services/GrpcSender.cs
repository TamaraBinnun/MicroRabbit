using MicroRabbit.Domain.Core.Dtos;
using MicroRabbit.Domain.Core.Interfaces;

namespace MicroRabbit.Infrastructure.Synchronous.Services
{
    public class GrpcSender : ISynchronousSender
    {
        public Task<bool> UpdateDataAsync<T>(T entity, string url) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
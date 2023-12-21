using AutoMapper;
using MicroRabbit.Application.Interfaces;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Interfaces;

namespace MicroRabbit.Application.Services
{
    public class ReadService<T, TResponse> : IReadService<TResponse>
        where T : class
        where TResponse : class
    {
        private readonly IReadRepository<T> _repository;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public ReadService(IReadRepository<T> repository, IEventBus eventBus, IMapper mapper)
        {
            _repository = repository;
            _eventBus = eventBus;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TResponse>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TResponse>>(entities);
        }

        public async Task<TResponse?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return null;
            }

            return _mapper.Map<TResponse>(entity);
        }

        public async Task<IEnumerable<TResponse>?> GetManyByIdAsync(IEnumerable<int> ids)
        {
            var entities = await _repository.GetManyByIdAsync(ids);
            if (entities == null)
            {
                return null;
            }

            return _mapper.Map<IEnumerable<TResponse>>(entities);
        }

        public async Task<bool> IsExistByIdAsync(int id)
        {
            var exist = await _repository.IsExistByIdAsync(id);

            return exist;
        }
    }
}
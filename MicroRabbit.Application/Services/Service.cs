using AutoMapper;
using MicroRabbit.Application.Interfaces;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Interfaces;

namespace MicroRabbit.Application.Services
{
    public class Service<T, TResponse, AddTRequest, UpdateTRequest> :
                                ReadService<T, TResponse>,
                                IService<TResponse, AddTRequest, UpdateTRequest>
        where T : class
        where TResponse : class
        where AddTRequest : class
        where UpdateTRequest : class
    {
        private readonly IRepository<T> _repository;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public Service(IRepository<T> repository, IEventBus eventBus, IMapper mapper) :
            base(repository, eventBus, mapper)
        {
            _repository = repository;
            _eventBus = eventBus;
            _mapper = mapper;
        }

        public async Task<TResponse> AddAsync(AddTRequest addRequest)
        {
            var entity = _mapper.Map<T>(addRequest);

            _repository.Add(entity);
            await _repository.SaveChangesAsync();

            var response = _mapper.Map<TResponse>(entity);//entity with the new Id

            return response;
        }

        public async Task<int> UpdateAsync(UpdateTRequest updateRequest)
        {
            var entity = _mapper.Map<T>(updateRequest);
            _repository.Update(entity);
            return await _repository.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return -1;
            }

            _repository.Delete(entity);
            return await _repository.SaveChangesAsync();
        }
    }
}
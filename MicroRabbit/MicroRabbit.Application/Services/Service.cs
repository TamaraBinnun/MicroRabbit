using AutoMapper;
using MicroRabbit.Application.Interfaces;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Interfaces;
using System.Linq.Expressions;

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
            var saveResponse = await _repository.SaveChangesAsync();

            var response = _mapper.Map<TResponse>(entity);//entity with the new Id

            return response;
        }

        public async Task<IEnumerable<TResponse>> AddManyAsync(IEnumerable<AddTRequest> addRequest)
        {
            var entities = _mapper.Map<IEnumerable<T>>(addRequest).ToList();

            entities.ForEach(_repository.Add);

            await _repository.SaveChangesAsync();

            var response = _mapper.Map<IEnumerable<TResponse>>(entities);//entity with the new Id

            return response;
        }

        public async Task<int> UpdateAsync(UpdateTRequest updateRequest)
        {
            var entity = _mapper.Map<T>(updateRequest);
            try
            {
                _repository.Update(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in repository update: {ex.Message}");
                throw;
            }

            return await _repository.SaveChangesAsync();
        }

        public async Task<int> UpdateManyAsync(IEnumerable<UpdateTRequest> updateRequest)
        {
            var entities = _mapper.Map<IEnumerable<T>>(updateRequest).ToList();

            entities.ForEach(_repository.Update);

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

        public async Task<int> DeleteManyAsync(IEnumerable<int> ids)
        {
            var entities = await _repository.GetManyByIdAsync(ids);
            if (entities == null)
            {
                return -1;
            }

            entities.ToList().ForEach(_repository.Delete);

            return await _repository.SaveChangesAsync();
        }

        public async Task<int> DeleteManyByFilterAsync(Expression<Func<TResponse, bool>> filter)
        {
            var filterT = _mapper.Map<Expression<Func<T, bool>>>(filter);
            var entities = _repository.GetMany(
                filter: filterT);

            if (entities == null)
            {
                return -1;
            }

            entities.ToList().ForEach(_repository.Delete);

            return await _repository.SaveChangesAsync();
        }
    }
}
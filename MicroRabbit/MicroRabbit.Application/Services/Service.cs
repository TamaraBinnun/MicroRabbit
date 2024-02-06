using AutoMapper;
using MicroRabbit.Application.Interfaces;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Interfaces;
using MicroRabbit.Domain.Core.Models;
using System.Linq.Expressions;

namespace MicroRabbit.Application.Services
{
    public class Service<T, TResponse, AddTRequest, UpdateTRequest> : IService<TResponse, AddTRequest, UpdateTRequest>
        where T : BaseModel
        where TResponse : BaseResponse
        where AddTRequest : class
        where UpdateTRequest : UpdateBaseRequest
    {
        private readonly IRepository<T, UpdateTRequest> _repository;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public Service(IRepository<T, UpdateTRequest> repository, IEventBus eventBus, IMapper mapper)
        {
            _repository = repository;
            _eventBus = eventBus;
            _mapper = mapper;
        }

        public IEnumerable<TResponse>? GetAll(Expression<Func<TResponse, bool>> filter = null,
                                       Expression<Func<IQueryable<TResponse>, IOrderedQueryable<TResponse>>> orderBy = null,
                                       int? top = null,
                                       int? skip = null,
                                       params string[] includeProperties)
        {
            var filterT = _mapper.Map<Expression<Func<T, bool>>>(filter);
            var orderByT = _mapper.Map<Expression<Func<IQueryable<T>, IOrderedQueryable<T>>>>(orderBy);
            var entities = _repository.GetAll(filterT, orderByT, top, skip, includeProperties);
            return _mapper.Map<IEnumerable<TResponse>?>(entities);
        }

        public async Task<TResponse?> GetByIdAsync(int id, params string[] includeProperties)
        {
            var entity = await _repository.GetByIdAsync(id, includeProperties);

            return _mapper.Map<TResponse?>(entity);
        }

        public async Task<IEnumerable<TResponse>?> GetByIdsAsync(IEnumerable<int>? ids)
        {
            if (ids == null) { return null; }

            var entities = await _repository.GetByIdsAsync(ids);
            return _mapper.Map<IEnumerable<TResponse>?>(entities);
        }

        public async Task<bool> IsExistByIdAsync(int id)
        {
            var response = await _repository.IsExistByIdAsync(id);
            return response;
        }

        public async Task<TResponse> AddAsync(AddTRequest addRequest)
        {
            var entity = _mapper.Map<T>(addRequest);

            _repository.Add(entity);
            var saveResponse = await _repository.SaveChangesAsync();

            var response = _mapper.Map<TResponse>(entity);//entity with the new Id

            return response;
        }

        public async Task<IEnumerable<TResponse>?> AddRangeAsync(IEnumerable<AddTRequest>? addRequest)
        {
            if (addRequest == null) { return null; }

            var entities = _mapper.Map<IEnumerable<T>>(addRequest).ToList();

            _repository.AddRange(entities);

            var saveResponse = await _repository.SaveChangesAsync();

            var response = _mapper.Map<IEnumerable<TResponse>>(entities);//entitues with the new Id

            return response;
        }

        public async Task<TResponse?> UpdateAsync(int id, UpdateTRequest updateRequest, params string[] includeProperties)
        {
            var currentEntity = await _repository.GetByIdAsync(id, includeProperties);
            if (currentEntity == null)
            {
                return null;
            }

            _repository.Update(currentEntity, updateRequest);

            var saveResponse = await _repository.SaveChangesAsync();

            var response = _mapper.Map<TResponse>(currentEntity);//entity with the updated data

            return response;
        }

        public async Task<TResponse?> DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return null;
            }
            //לנסות לשלוח רק מספרי ID ללא שאר מידע בשביל מחיקה

            _repository.Delete(entity);

            var saveResponse = await _repository.SaveChangesAsync();

            var response = _mapper.Map<TResponse>(entity);
            return response;
        }

        public async Task<IEnumerable<TResponse>?> DeleteRange(IEnumerable<int>? ids)
        {
            if (ids == null) { return null; }

            var entities = await _repository.GetByIdsAsync(ids);
            if (entities == null)
            {
                return null;
            }
            //לנסות לשלוח רק מספרי ID ללא שאר מידע בשביל מחיקה
            _repository.DeleteRange(entities);

            var saveResponse = await _repository.SaveChangesAsync();

            var response = _mapper.Map<IEnumerable<TResponse>>(entities);
            return response;
        }

        public async Task<IEnumerable<TResponse>?> DeleteManyByFilterAsync(Expression<Func<TResponse, bool>> filter)
        {
            var filterT = _mapper.Map<Expression<Func<T, bool>>>(filter);
            var entities = _repository.GetAll(filter: filterT);

            if (entities == null)
            {
                return null;
            }

            _repository.DeleteRange(entities);

            var saveResponse = await _repository.SaveChangesAsync();

            var response = _mapper.Map<IEnumerable<TResponse>>(entities);
            return response;
        }
    }
}
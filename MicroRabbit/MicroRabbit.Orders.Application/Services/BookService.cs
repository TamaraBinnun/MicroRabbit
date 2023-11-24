using AutoMapper;
using MicroRabbit.Orders.Application.Dtos;
using MicroRabbit.Orders.Application.Interfaces;
using MicroRabbit.Orders.Domain.Interfaces;
using MicroRabbit.Orders.Domain.Models;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Application.Services;

namespace MicroRabbit.Orders.Application.Services
{
    public class BookService : ReadService<Book, BookResponse>, IBookService
    {
        private readonly IBookRepository _repository;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public BookService(IBookRepository repository, IEventBus eventBus, IMapper mapper)
             : base(repository, eventBus, mapper)
        {
            _repository = repository;
            _eventBus = eventBus;
            _mapper = mapper;
        }
    }
}
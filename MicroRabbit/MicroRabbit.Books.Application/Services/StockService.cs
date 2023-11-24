using AutoMapper;
using MicroRabbit.Application.Services;
using MicroRabbit.Books.Application.Dtos;
using MicroRabbit.Books.Application.Interfaces;
using MicroRabbit.Books.Domain.Interfaces;
using MicroRabbit.Books.Domain.Models;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Dtos;

namespace MicroRabbit.Books.Application.Services
{
    public class StockService : Service<BookInStock, StockResponse, BookUnits, UpdateStockRequest>, IStockService
    {
        private readonly IStockRepository _repository;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public StockService(IStockRepository repository, IEventBus eventBus, IMapper mapper)
            : base(repository, eventBus, mapper)
        {
            _repository = repository;
            _eventBus = eventBus;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookUnits>> GetBookUnitsInStockAsync(List<int> bookIds)
        {
            bookIds = bookIds.Distinct().ToList();
            var booksInStock = await _repository.GetAllAsync();
            var selectedInStock = (from b in bookIds
                                   join i in booksInStock on b equals i.BookId into temp
                                   from l in temp.DefaultIfEmpty()
                                   select l).ToList();
            var response = _mapper.Map<IEnumerable<BookUnits>>(selectedInStock);

            return response;
        }

        public async Task<int> UpdateBookUnitsInStockAsync(List<BookUnits> bookUnits)
        {
            var tasks = bookUnits.Select(async x =>
            {
                var bookInStock = await _repository.GetByBookIdAsync(x.BookId);
                if (bookInStock != null)
                {
                    bookInStock.Units += x.Units;
                    _repository.Update(bookInStock);
                }
            }).ToList();

            await Task.WhenAll(tasks);
            return await _repository.SaveChangesAsync();
        }
    }
}
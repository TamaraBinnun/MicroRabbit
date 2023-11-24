using MicroRabbit.Books.Application.Interfaces;
using MicroRabbit.Domain.Core.EventHandlers;
using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Books.Application.EventHandlers
{
    public class EventToUpdateStockHandler : IEventHandler<EventToUpdateStock>
    {
        private readonly IStockService _stockService;

        public EventToUpdateStockHandler(IStockService stockService)
        {
            _stockService = stockService;
        }

        public async Task<int> Handle(EventToUpdateStock @event)
        {
            var response = await _stockService.UpdateBookUnitsInStockAsync(@event.BookUnits);
            return response;
        }
    }
}
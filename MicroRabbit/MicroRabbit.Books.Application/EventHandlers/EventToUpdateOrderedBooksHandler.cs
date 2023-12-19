using MicroRabbit.Books.Application.Interfaces;
using MicroRabbit.Domain.Core.Dtos;
using MicroRabbit.Domain.Core.EventHandlers;
using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Books.Application.EventHandlers
{
    public class EventToUpdateOrderedBooksHandler : IEventHandler<EventToUpdateOrderedBooks>
    {
        private readonly IOrderedBooksService _orderedBooksService;

        public EventToUpdateOrderedBooksHandler(IOrderedBooksService orderedBooksService)
        {
            _orderedBooksService = orderedBooksService;
        }

        public async Task Handle(EventToUpdateOrderedBooks @event)
        {
            await _orderedBooksService.UseEventToUpdateOrderedBooksAsync(@event.BookUnits);
        }
    }
}
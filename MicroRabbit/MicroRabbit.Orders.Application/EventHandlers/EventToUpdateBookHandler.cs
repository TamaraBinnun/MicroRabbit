using MicroRabbit.Domain.Core.EventHandlers;
using MicroRabbit.Domain.Core.Events;
using MicroRabbit.Orders.Application.Interfaces;

namespace MicroRabbit.Orders.Application.EventHandlers
{
    public class EventToUpdateBookHandler : IEventHandler<EventToUpdateBook>
    {
        private readonly IBooksService _bookService;

        public EventToUpdateBookHandler(IBooksService bookService)
        {
            _bookService = bookService;
        }

        public async Task Handle(EventToUpdateBook @event)
        {
            await _bookService.UseEventToUpdateBookAsync(@event.BookData);
        }
    }
}
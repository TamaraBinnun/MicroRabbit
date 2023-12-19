using MediatR;
using MicroRabbit.Books.Domain.Commands;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Books.Domain.CommandHandlers
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, bool>
    {
        private readonly IEventBus _eventBus;

        public UpdateBookCommandHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task<bool> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {//publish event to RabbitMQ
            var response = _eventBus.Publish(new EventToUpdateBook(request.BookData));
            return Task.FromResult(response);
        }
    }
}
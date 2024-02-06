using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Events;
using MicroRabbit.Orders.Domain.Commands;

namespace MicroRabbit.Orders.Domain.CommandHandlers
{
    public class UpdateOrderedBooksCommandHandler : IRequestHandler<UpdateOrderedBooksCommand, bool>
    {
        private readonly IEventBus _eventBus;

        public UpdateOrderedBooksCommandHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task<bool> Handle(UpdateOrderedBooksCommand request, CancellationToken cancellationToken)
        {//publish event to RabbitMQ
            var response = _eventBus.Publish(new EventToUpdateOrderedBooks(request.CommonOrder));
            return Task.FromResult(response);
        }
    }
}
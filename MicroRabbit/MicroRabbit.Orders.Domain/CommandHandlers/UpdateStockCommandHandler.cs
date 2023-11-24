using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Events;
using MicroRabbit.Orders.Domain.Commands;

namespace MicroRabbit.Orders.Domain.CommandHandlers
{
    public class UpdateStockCommandHandler : IRequestHandler<UpdateStockCommand, bool>
    {
        private readonly IEventBus _eventBus;

        public UpdateStockCommandHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task<bool> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
        {//publish event to RabbitMQ
            var response = _eventBus.Publish(new EventToUpdateStock(request.BookUnits));
            return Task.FromResult(response);
        }
    }
}
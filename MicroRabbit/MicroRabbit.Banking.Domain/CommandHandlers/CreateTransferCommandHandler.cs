using MediatR;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Events;
using MicroRabbit.Domain.Core.Bus;

namespace MicroRabbit.Banking.Domain.CommandHandlers
{
    public class CreateTransferCommandHandler : IRequestHandler<CreateTransferCommand, bool>
    {
        private readonly IEventBus _eventBus;

        public CreateTransferCommandHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task<bool> Handle(CreateTransferCommand request, CancellationToken cancellationToken)
        {//publish event to RabbitMQ
            _eventBus.Publish(new EventToCreateTransfer(request.From, request.To, request.Amount));
            return Task.FromResult(true);
        }
    }
}
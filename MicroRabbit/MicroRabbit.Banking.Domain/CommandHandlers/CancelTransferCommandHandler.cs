using MediatR;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Domain.Core.Bus;

namespace MicroRabbit.Banking.Domain.CommandHandlers
{
    public class CancelTransferCommandHandler : IRequestHandler<CancelTransferCommand, bool>
    {
        private readonly IEventBus _eventBus;

        public CancelTransferCommandHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task<bool> Handle(CancelTransferCommand request, CancellationToken cancellationToken)
        {//publish event to RabbitMQ
            return Task.FromResult(true);
        }
    }
}
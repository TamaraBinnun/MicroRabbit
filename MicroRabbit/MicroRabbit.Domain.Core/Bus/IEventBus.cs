using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.EventHandlers;
using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Domain.Core.Bus
{
    public interface IEventBus
    {
        Task<bool> SendCommand<C>(C command) where C : Command;

        bool Publish<E>(E @event) where E : Event;

        void Subscribe<E, EH>()
            where E : Event
            where EH : IEventHandler<E>;
    }
}
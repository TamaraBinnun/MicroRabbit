using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Domain.Core.EventHandlers
{
    public interface IEventHandler<E>
        where E : Event
    {
        Task Handle(E @event);
    }
}
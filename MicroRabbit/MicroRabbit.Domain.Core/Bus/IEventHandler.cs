using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Domain.Core.Bus
{
    public interface IEventHandler<E>
        where E : Event
    {
        Task Handle(E @event);
    }
}
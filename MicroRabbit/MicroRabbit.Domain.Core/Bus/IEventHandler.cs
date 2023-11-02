using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Domain.Core.Bus
{
    public interface IEventHandler<in E> : IEventHandler
        where E : Event
    {
        Task Handle(E @event);
    }

    public interface IEventHandler
    {
    }
}
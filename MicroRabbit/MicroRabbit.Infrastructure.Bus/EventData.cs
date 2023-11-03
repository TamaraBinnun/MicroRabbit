namespace MicroRabbit.Infrastructure.Bus
{
    public class EventData
    {
        public Type EventType { get; set; } = null!;
        public List<Type> EventHandlersType { get; set; } = null!;
    }
}
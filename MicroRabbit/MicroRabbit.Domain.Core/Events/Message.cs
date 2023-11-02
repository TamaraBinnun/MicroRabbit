using MediatR;

namespace MicroRabbit.Domain.Core.Events
{
    public abstract class Message : IRequest<bool>
    {
        public string MessageType { get; protected set; }
        public string SenderName { get; protected set; } = string.Empty;
        public string MessageDescription { get; protected set; } = string.Empty;

        protected Message()
        {
            MessageType = GetType().Name;
        }
    }
}
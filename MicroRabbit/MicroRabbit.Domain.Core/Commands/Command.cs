using MediatR;

namespace MicroRabbit.Domain.Core.Commands
{
    public abstract class Command : IRequest<bool>
    {
        public string CommandeType { get; protected set; }
        public string SenderName { get; protected set; } = string.Empty;
        public string CommandDescription { get; protected set; } = string.Empty;

        public DateTime Timestamp { get; protected set; }

        protected Command()
        {
            CommandeType = GetType().Name;
            Timestamp = DateTime.Now;
        }
    }
}
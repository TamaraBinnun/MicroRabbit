using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection;
using System.Text;

namespace MicroRabbit.Infrastructure.Bus
{
    public sealed class RabbitMQBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly Dictionary<string, EventData> _eventData;//event name => type + all it's subscribers

        public RabbitMQBus(IMediator mediator)
        {
            _mediator = mediator;
            _eventData = new Dictionary<string, EventData>();
        }

        public Task SendCommand<C>(C command) where C : Command
        {
            return _mediator.Send(command);
        }

        public void Publish<E>(E @event) where E : Event
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var eventName = @event.GetType().Name;
                    channel.QueueDeclare(eventName, false, false, false, null);
                    string message = JsonConvert.SerializeObject(@event);
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish("", eventName, null, body);
                }
            }
            Console.WriteLine("The message was sent");
        }

        public void Subscribe<E, EH>()
            where E : Event
            where EH : IEventHandler<E>
        {
            SubscribeValidate<E, EH>();
            SubscribeConsumer<E>();
        }

        private void SubscribeValidate<E, EH>()
            where E : Event
            where EH : IEventHandler<E>
        {
            var eventType = typeof(E);
            var handlerType = typeof(EH);

            if (!_eventData.ContainsKey(eventType.Name))
            {// no event name in dictionary
                _eventData.Add(eventType.Name, new EventData()
                {
                    EventType = eventType,
                    EventHandlersType = new List<Type> { handlerType }
                });
            }
            else if (!_eventData[eventType.Name].EventHandlersType.Contains(handlerType))
            {// event name exist but subcriber does not exist
                _eventData[eventType.Name].EventHandlersType.Add(handlerType);
            }
            else
            {
                throw new ArgumentException($"Handler type {handlerType.Name} is already registered for event {eventType.Name}", handlerType.Name);
            }
        }

        private void SubscribeConsumer<E>() where E : Event
        {
            var eventName = typeof(E).Name;

            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                DispatchConsumersAsync = true
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(eventName, false, false, false, null);
                    var consumer = new AsyncEventingBasicConsumer(channel);
                    consumer.Received += ConsumerReceived;
                    channel.BasicConsume(eventName, true, consumer);
                }
            }
            Console.WriteLine("The message was received");
        }

        private async Task ConsumerReceived(object sender, BasicDeliverEventArgs @event)
        {//executes when the message consumes from the queue
            try
            {//execute all event handlers for this event
                await ProcessEvent(@event.RoutingKey, @event.Body.ToArray()).ConfigureAwait(false);
            }
            catch (Exception /*ex*/)
            {
            }
        }

        private async Task ProcessEvent(string eventName, byte[] eventBody)
        {
            if (!_eventData.ContainsKey(eventName))
            {
                return; //no subscribers to this event
            }

            foreach (var eventHandlerType in _eventData[eventName].EventHandlersType)
            {
                var eventHandler = Activator.CreateInstance(eventHandlerType);
                if (eventHandler == null) { continue; }

                var message = Encoding.UTF8.GetString(eventBody);
                var eventType = _eventData[eventName].EventType;
                var @event = JsonConvert.DeserializeObject(message, eventType)!;

                MethodInfo method = eventHandlerType.GetMethod("Handle")!;
                method.Invoke(eventHandler, new object[] { @event });

                //Example
                /* Event name:"CreateUserCommand"
                 * EventType:CreateUserCommand
                 * EventHandlerType:CreateUserCommandHandler
                 * */
            }
        }
    }
}
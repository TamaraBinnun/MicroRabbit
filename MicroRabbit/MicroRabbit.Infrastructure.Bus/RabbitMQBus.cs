using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.EventHandlers;
using MicroRabbit.Domain.Core.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MicroRabbit.Infrastructure.Bus
{
    public sealed class RabbitMQBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _config;
        private readonly Dictionary<string, EventData> _eventData;//event name => type + all it's subscribers

        public RabbitMQBus(IMediator mediator, IServiceScopeFactory serviceScopeFactory, IConfiguration config)
        {
            _mediator = mediator;
            _serviceScopeFactory = serviceScopeFactory;
            _config = config;
            _eventData = new Dictionary<string, EventData>();
        }

        public async Task<bool> SendCommand<C>(C command) where C : Command
        {
            var response = await _mediator.Send(command);
            return response;
        }

        public bool Publish<E>(E @event) where E : Event
        {
            var response = false;
            try
            {
                var factory = new ConnectionFactory()
                {
                    Uri = new Uri(_config.GetConnectionString("RabbitMqConnection")!),

                    /*HostName = "host.docker.internal",
                    UserName = _config["RabbitMQ:UserName"],
                    Password = _config["RabbitMQ:Password"],*/
                };

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
                response = true;
            }
            catch
            {
                //write to log
            }
            return response;
        }

        //eventBus.Subscribe<EventToCreateTransfer, EventToCreateTransferHandler>()
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
                Uri = new Uri(_config.GetConnectionString("RabbitMqConnection")!),
                DispatchConsumersAsync = true

                /*HostName = "host.docker.internal",
                UserName = _config["RabbitMQ:UserName"],
                Password = _config["RabbitMQ:Password"],*/
            };

            var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.QueueDeclare(eventName, false, false, false, null);
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += ConsumerReceived;
            channel.BasicConsume(eventName, true, consumer);

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

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                foreach (var eventHandlerType in _eventData[eventName].EventHandlersType)
                {
                    var eventHandler = scope.ServiceProvider.GetService(eventHandlerType);
                    //Activator.CreateInstance(eventHandlerType); - not good because need empty ctor for EventToCreateTransferHandler without injected objects
                    if (eventHandler == null) { continue; }

                    var message = Encoding.UTF8.GetString(eventBody);
                    var eventType = _eventData[eventName].EventType;
                    var @event = JsonConvert.DeserializeObject(message, eventType)!;

                    var method = eventHandlerType.GetMethod("Handle")!;
                    await (Task)method.Invoke(eventHandler, new object[] { @event })!;

                    //Example
                    /* Event name:"EventToCreateTransfer"
                     * EventType:EventToCreateTransfer
                     * EventHandlerType:EventToCreateTransferHandler
                     * */
                }
            }
        }
    }
}
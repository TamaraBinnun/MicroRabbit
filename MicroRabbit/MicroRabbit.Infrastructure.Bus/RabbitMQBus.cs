using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.EventHandlers;
using MicroRabbit.Domain.Core.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace MicroRabbit.Infrastructure.Bus
{
    public sealed class RabbitMQBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _config;
        private readonly Dictionary<string, EventData> _eventData;//event name => type + all it's subscribers
        private readonly IModel? _channel;
        private readonly IConnection? _connection;

        public RabbitMQBus(IMediator mediator, IServiceScopeFactory serviceScopeFactory, IConfiguration config)
        {
            _mediator = mediator;
            _serviceScopeFactory = serviceScopeFactory;
            _config = config;
            _eventData = new Dictionary<string, EventData>();
            var factory = GetConnectionFactory();

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _connection.ConnectionShutdown += Connection_ConnectionShutdown;
                Console.WriteLine("Connected to RabbitMQBus");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Couldn't connect to the RabbitMQBus: {ex.Message}");
            }
        }

        private ConnectionFactory GetConnectionFactory()
        {
            Console.WriteLine($"HostName={_config["RabbitMq:HostName"]}");
            Console.WriteLine($"UserName={_config["RabbitMQ:UserName"]}");

            var factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMq:HostName"],
                Port = int.Parse(_config["RabbitMq:Port"]!),
                UserName = _config["RabbitMQ:UserName"],
                Password = _config["RabbitMQ:Password"],
                Ssl = new SslOption
                {
                    Enabled = false
                },
                DispatchConsumersAsync = true,
            };

            return factory;
        }

        private void Connection_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine($"RabbitMQBus Connection Shutdown");
        }

        public void Dispose()
        {
            Console.WriteLine($"RabbitMQBus was disposed");
            if (_channel != null && _channel.IsOpen)
            {
                _channel.Close();
            }
            if (_connection != null && _connection.IsOpen)
            {
                _connection.Close();
            }
        }

        public async Task<bool> SendCommand<C>(C command) where C : Command
        {
            var response = await _mediator.Send(command);
            return response;
        }

        public bool Publish<E>(E @event) where E : Event
        {
            if (_channel == null) throw new NullReferenceException($"{nameof(_channel)} is null");
            if (!_channel.IsOpen) throw new ApplicationException("Publish channel was closed");

            var response = false;
            try
            {
                var eventName = @event.GetType().Name;
                _channel.QueueDeclare(eventName, false, false, false, null);
                string message = JsonSerializer.Serialize(@event);
                var body = Encoding.UTF8.GetBytes(message);
                _channel.BasicPublish("", eventName, null, body);

                Console.WriteLine("The message was sent");
                response = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The message was not sent: {ex.Message}");
            }
            return response;
        }

        //eventBus.Subscribe<EventToCreateTransfer, EventToCreateTransferHandler>()
        public void Subscribe<E, EH>()
            where E : Event
            where EH : IEventHandler<E>
        {
            if (_channel == null) throw new NullReferenceException($"{nameof(_channel)} is null");
            if (!_channel.IsOpen) throw new ApplicationException("Subscribe channel was closed");

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

            _channel!.QueueDeclare(eventName, false, false, false, null);
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += ConsumerReceived;
            _channel.BasicConsume(eventName, true, consumer);

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
                    var @event = JsonSerializer.Deserialize(message, eventType)!;

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
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.EventHandlers;
using MicroRabbit.Domain.Core.Events;
using MicroRabbit.Domain.Core.Interfaces;
using MicroRabbit.Infrastructure.IoC;
using MicroRabbit.Infrastructure.Synchronous.Services;
using MicroRabbit.Orders.Application.Dtos.Books;
using MicroRabbit.Orders.Application.Dtos.OrderItems;
using MicroRabbit.Orders.Application.Dtos.Orders;
using MicroRabbit.Orders.Application.EventHandlers;
using MicroRabbit.Orders.Application.Interfaces;
using MicroRabbit.Orders.Application.Services;
using MicroRabbit.Orders.Data.Context;
using MicroRabbit.Orders.Data.Repository;
using MicroRabbit.Orders.Domain.CommandHandlers;
using MicroRabbit.Orders.Domain.Commands;
using MicroRabbit.Orders.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace MicroRabbit.Orders.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<OrderDbContext>(options =>
            {
                var server = builder.Configuration.GetConnectionString("Server");
                var database = builder.Configuration.GetConnectionString("Database");
                var user = builder.Configuration.GetConnectionString("User");
                var password = builder.Configuration.GetConnectionString("Password");
                var dbConnection = string.Format(builder.Configuration.GetConnectionString("Template")!, server, database, user, password);
                options.UseSqlServer(dbConnection);
            }, ServiceLifetime.Transient);

            builder.Services.AddHttpClient<ISynchronousSender, HttpSender>((httpClient, sp) =>
            {
                httpClient.BaseAddress = new Uri(builder.Configuration["MicroRabbitBooks:BaseAddress"]!);
                return new HttpSender(httpClient);
            });

            // Add services to the container.

            builder.Services.AddControllers();

            var assembly = Assembly.Load("MicroRabbit.Orders.Application");
            builder.Services.AddAutoMapper(cfg => { cfg.AddExpressionMapping(); }, assembly);

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order Microservice", Version = "v1" });
            });

            builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            RegisterServices(builder.Services);

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order Microservice v1");
            });

            ConfigureEventBus(app);

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void ConfigureEventBus(WebApplication app)
        { //configure microservices to subscribe to event
            var eventBus = app.Services.GetRequiredService<IEventBus>();
            eventBus.Subscribe<EventToUpdateBook, EventToUpdateBookHandler>();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            DependencyContainer.RegisterServices(services);

            services.AddTransient<IBooksService, BooksService>();
            services.AddTransient<IBooksRepository<UpdateBookRequest>, BookRepository<UpdateBookRequest>>();

            services.AddTransient<IOrdersService, OrdersService>();
            services.AddTransient<IOrdersRepository<UpdateOrderRequest, UpdateOrderItemRequest>, OrderRepository<UpdateOrderRequest, UpdateOrderItemRequest>>();

            services.AddTransient<IOrderItemsService, OrderItemsService>();
            services.AddTransient<IOrderItemsRepository<UpdateOrderItemRequest>, OrderItemRepository<UpdateOrderItemRequest>>();

            services.AddTransient<IRequestHandler<UpdateOrderedBooksCommand, bool>, UpdateOrderedBooksCommandHandler>();

            services.AddTransient<IEventHandler<EventToUpdateBook>, EventToUpdateBookHandler>();
            services.AddTransient<EventToUpdateBookHandler>();
        }
    }
}
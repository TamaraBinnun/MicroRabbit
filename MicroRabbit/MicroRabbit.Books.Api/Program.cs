using MediatR;
using MicroRabbit.Books.Application.EventHandlers;
using MicroRabbit.Books.Application.Interfaces;
using MicroRabbit.Books.Application.Services;
using MicroRabbit.Books.Data.Context;
using MicroRabbit.Books.Data.Repository;
using MicroRabbit.Books.Domain.Commands;
using MicroRabbit.Books.Domain.CommandHandlers;
using MicroRabbit.Books.Domain.Interfaces;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.EventHandlers;
using MicroRabbit.Domain.Core.Events;
using MicroRabbit.Domain.Core.Interfaces;
using MicroRabbit.Infrastructure.IoC;
using MicroRabbit.Infrastructure.Synchronous.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using System.Reflection;

namespace MicroRabbit.Books.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<BookDbContext>(options =>
            {
                var server = builder.Configuration.GetConnectionString("Server");
                var database = builder.Configuration.GetConnectionString("Database");
                var user = builder.Configuration.GetConnectionString("User");
                var password = builder.Configuration.GetConnectionString("Password");
                var dbConnection = string.Format(builder.Configuration.GetConnectionString("Template")!, server, database, user, password);
                options.UseSqlServer(dbConnection);
            });

            builder.Services.AddHttpClient<ISynchronousSender, HttpSender>((httpClient, sp) =>
            {
                httpClient.BaseAddress = new Uri(builder.Configuration["MicroRabbitOrders:BaseAddress"]!);
                return new HttpSender(httpClient);
            });

            // Add services to the container.

            builder.Services.AddControllers();

            var assembly = Assembly.Load("MicroRabbit.Books.Application");
            builder.Services.AddAutoMapper(assembly);

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Book Microservice", Version = "v1" });
            });

            builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            RegisterServices(builder.Services);

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Book Microservice v1");
            });

            ConfigureEventBus(app);

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void ConfigureEventBus(WebApplication app)
        {//configure microservices to subscribe to event
            var eventBus = app.Services.GetRequiredService<IEventBus>();
            eventBus.Subscribe<EventToUpdateOrderedBooks, EventToUpdateOrderedBooksHandler>();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            DependencyContainer.RegisterServices(services);

            services.AddTransient<IBooksService, BooksService>();
            services.AddTransient<IBooksRepository, BooksRepository>();

            services.AddTransient<IOrderedBooksService, OrderedBooksService>();
            services.AddTransient<IOrderedBooksRepository, OrderedBooksRepository>();

            services.AddTransient<BookDbContext>();

            services.AddTransient<IRequestHandler<UpdateBookCommand, bool>, UpdateBookCommandHandler>();

            services.AddTransient<IEventHandler<EventToUpdateOrderedBooks>, EventToUpdateOrderedBooksHandler>();
            services.AddTransient<EventToUpdateOrderedBooksHandler>();
        }
    }
}
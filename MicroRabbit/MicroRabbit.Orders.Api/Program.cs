using MediatR;
using MicroRabbit.Infrastructure.IoC;
using MicroRabbit.Orders.Application.Interfaces;
using MicroRabbit.Orders.Application.Services;
using MicroRabbit.Orders.Data.Context;
using MicroRabbit.Orders.Data.Repository;
using MicroRabbit.Orders.Domain.CommandHandlers;
using MicroRabbit.Orders.Domain.Commands;
using MicroRabbit.Orders.Domain.Interfaces;
using MicroRabbit.Orders.Infrastructure.Synchronous.Services;
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
            });

            builder.Services.AddHttpClient<IMicroRabbitBooksClient, HttpMicroRabbitBooksClient>((httpClient, sp) =>
            {
                httpClient.BaseAddress = new Uri(builder.Configuration["MicroRabbitBooks:BaseAddress"]!);
                return new HttpMicroRabbitBooksClient(httpClient, sp.GetService<IConfiguration>()!);

                // The GitHub API requires two headers.
                /*httpClient.DefaultRequestHeaders.Add(
                    HeaderNames.Accept, "application/vnd.github.v3+json");
                httpClient.DefaultRequestHeaders.Add(
                    HeaderNames.UserAgent, "HttpRequestsSample");*/
            });

            // Add services to the container.

            builder.Services.AddControllers();

            var assembly = Assembly.Load("MicroRabbit.Orders.Application");
            builder.Services.AddAutoMapper(assembly);

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

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            DependencyContainer.RegisterServices(services);

            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IBookRepository, BookRepository>();

            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IOrderRepository, OrderRepository>();

            services.AddTransient<OrderDbContext>();

            services.AddTransient<IRequestHandler<UpdateStockCommand, bool>, UpdateStockCommandHandler>();
        }
    }
}
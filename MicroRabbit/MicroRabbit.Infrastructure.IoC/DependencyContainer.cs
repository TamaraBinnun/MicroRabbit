using MediatR;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Data.Repository;
using MicroRabbit.Banking.Domain.CommandHandlers;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Application.Services;
using MicroRabbit.Transfer.Data.Repository;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infrastructure.Bus;
using Microsoft.Extensions.DependencyInjection;
using MicroRabbit.Transfer.Domain.EventHandles;
using MicroRabbit.Transfer.Domain.Events;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Banking.Data.Context;

namespace MicroRabbit.Infrastructure.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IEventBus, RabbitMQBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitMQBus(sp.GetService<IMediator>()!, scopeFactory);
            });

            //Domain Banking Commands
            services.AddTransient<IRequestHandler<CreateTransferCommand, bool>, CreateTransferCommandHandler>();

            //Domain Events
            services.AddTransient<IEventHandler<EventToCreateTransfer>, EventToCreateTransferHandler>();
            services.AddTransient<EventToCreateTransferHandler>();

            //Application Services
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ITransferService, TransferService>();

            //Data
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<ITransferRepository, TransferRepository>();

            services.AddTransient<BankDbContext>();
            services.AddTransient<TransferDbContext>();
        }
    }
}
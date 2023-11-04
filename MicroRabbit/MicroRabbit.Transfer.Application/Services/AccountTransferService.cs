using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;
using MicroRabbit.Domain.Core.Bus;

namespace MicroRabbit.Transfer.Application.Services
{
    public class AccountTransferService : IAccountTransferService
    {
        private readonly IAccountTransferRepository _repository;
        private readonly IEventBus _eventBus;

        public AccountTransferService(IAccountTransferRepository repository, IEventBus eventBus)
        {
            _repository = repository;
            _eventBus = eventBus;
        }

        public IEnumerable<AccountTransfer> GetAccountTransfers()
        {
            return _repository.GetAccountTransfers();
        }
    }
}
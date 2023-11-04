using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Domain.Interfaces
{
    public interface IAccountTransferRepository
    {
        IEnumerable<AccountTransfer> GetAccountTransfers();
    }
}
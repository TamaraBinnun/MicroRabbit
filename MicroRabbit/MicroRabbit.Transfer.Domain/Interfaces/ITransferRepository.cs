using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Domain.Interfaces
{
    public interface ITransferRepository
    {
        IEnumerable<AccountTransfer> GetTransfers();

        void Add(AccountTransfer accountTransfer);
    }
}
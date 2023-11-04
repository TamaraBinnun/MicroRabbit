using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Data.Repository
{
    public class AccountTransferRepository : IAccountTransferRepository
    {
        private readonly TransferDbContext _context;

        public AccountTransferRepository(TransferDbContext context)
        {
            _context = context;
        }

        public IEnumerable<AccountTransfer> GetAccountTransfers()
        {
            return _context.AccountTransfers;
        }
    }
}
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Data.Repository
{
    public class TransferRepository : ITransferRepository
    {
        private readonly TransferDbContext _context;

        public TransferRepository(TransferDbContext context)
        {
            _context = context;
        }

        public void Add(AccountTransfer accountTransfer)
        {
            _context.AccountTransfers.Add(accountTransfer);
            _context.SaveChanges();
        }

        public IEnumerable<AccountTransfer> GetTransfers()
        {
            return _context.AccountTransfers;
        }
    }
}
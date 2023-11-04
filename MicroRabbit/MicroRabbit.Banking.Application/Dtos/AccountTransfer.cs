using System.Reflection.Emit;

namespace MicroRabbit.Banking.Application.Dtos
{
    public class AccountTransfer
    {
        public int FromAccount { get; set; }
        public int ToAccount { get; set; }
        public decimal TransferAmount { get; set; }
    }
}
namespace MicroRabbit.Banking.Domain.Commands
{
    public class CancelTransferCommand : TransferCommand
    {
        public CancelTransferCommand(int from, int to, decimal amount)
        {
            From = from;
            To = to;
            Amount = amount;
        }
    }
}
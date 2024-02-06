using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Dtos;

namespace MicroRabbit.Orders.Domain.Commands
{
    public class UpdateOrderedBooksCommand : Command
    {
        public UpdateOrderedBooksCommand(CommonOrder commonOrder)
        {
            CommonOrder = commonOrder;
        }

        public CommonOrder CommonOrder { get; set; }
    }
}
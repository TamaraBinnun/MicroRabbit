using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Dtos;

namespace MicroRabbit.Orders.Domain.Commands
{
    public class UpdateOrderedBooksCommand : Command
    {
        public UpdateOrderedBooksCommand(IEnumerable<CommonOrderedBook> bookUnits)
        {
            BookUnits = bookUnits;
        }

        public IEnumerable<CommonOrderedBook> BookUnits { get; set; }
    }
}
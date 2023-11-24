using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Dtos;

namespace MicroRabbit.Orders.Domain.Commands
{
    public class UpdateStockCommand : Command
    {
        public UpdateStockCommand(List<BookUnits> bookUnits)
        {
            BookUnits = bookUnits;
        }

        public List<BookUnits> BookUnits { get; set; }
    }
}
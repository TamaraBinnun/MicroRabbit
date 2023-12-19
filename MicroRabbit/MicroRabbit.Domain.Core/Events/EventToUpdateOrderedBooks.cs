using MicroRabbit.Domain.Core.Dtos;

namespace MicroRabbit.Domain.Core.Events
{
    public class EventToUpdateOrderedBooks : Event
    {
        public EventToUpdateOrderedBooks(IEnumerable<CommonOrderedBook> bookUnits)
        {
            BookUnits = bookUnits;
        }

        public IEnumerable<CommonOrderedBook> BookUnits { get; set; }
    }
}
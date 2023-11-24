using MicroRabbit.Domain.Core.Dtos;

namespace MicroRabbit.Domain.Core.Events
{
    public class EventToUpdateStock : Event
    {
        public EventToUpdateStock(List<BookUnits> bookUnits)
        {
            BookUnits = bookUnits;
        }

        public List<BookUnits> BookUnits { get; set; }
    }
}
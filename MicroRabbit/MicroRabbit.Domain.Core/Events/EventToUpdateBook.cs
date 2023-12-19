using MicroRabbit.Domain.Core.Dtos;

namespace MicroRabbit.Domain.Core.Events
{
    public class EventToUpdateBook : Event
    {
        public EventToUpdateBook(CommonBookData bookData)
        {
            BookData = bookData;
        }

        public CommonBookData BookData { get; set; }
    }
}
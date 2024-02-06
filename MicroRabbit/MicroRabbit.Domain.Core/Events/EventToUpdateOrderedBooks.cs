using MicroRabbit.Domain.Core.Dtos;

namespace MicroRabbit.Domain.Core.Events
{
    public class EventToUpdateOrderedBooks : Event
    {
        public EventToUpdateOrderedBooks(CommonOrder commonOrder)
        {
            CommonOrder = commonOrder;
        }

        public CommonOrder CommonOrder { get; set; }
    }
}
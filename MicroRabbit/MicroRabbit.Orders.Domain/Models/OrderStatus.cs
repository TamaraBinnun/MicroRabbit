namespace MicroRabbit.Orders.Domain.Models
{
    public enum OrderStatus
    {
        None = 0,
        Created = 1,//the order has been created
        Updated = 2,//the order has been updated
        StartHandling = 3,//when employer starts handling this order -  no updates can be done
        FinishHandling = 4,//when employer finishes handling this order - and the order is ready for delivery
        Delivered = 5
    }
}
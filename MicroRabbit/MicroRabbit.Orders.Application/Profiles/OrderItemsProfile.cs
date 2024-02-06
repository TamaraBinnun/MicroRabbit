using MicroRabbit.Application.Profiles;
using MicroRabbit.Domain.Core.Dtos;
using MicroRabbit.Orders.Application.Dtos;
using MicroRabbit.Orders.Application.Dtos.OrderItems;
using MicroRabbit.Orders.Domain.Models;

namespace MicroRabbit.Orders.Application.Profiles
{
    public class OrderItemsProfile : BaseProfile<OrderItem, OrderItemResponse, AddOrderItemRequest, UpdateOrderItemRequest>
    {
        public OrderItemsProfile()
        {
            //OrdersController => OrdersService => GetItemsToUpdate
            CreateMap<AddOrderItemRequest, UpdateOrderItemRequest>();

            //****OrdersController => GetOrderItemsCreateEvent**************
            CreateMap<(CommonOrderedBook, BookResponse), CommonOrderedBook>()
                 .IncludeMembers(s => s.Item1)
                 .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.Item2.ISBN));

            CreateMap<CommonOrderedBook, CommonOrderedBook>();
            CreateMap<BookResponse, CommonOrderedBook>();
            //**************************************************
            //.ReverseMap();
        }
    }
}
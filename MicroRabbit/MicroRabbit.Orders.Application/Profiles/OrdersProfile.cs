using MicroRabbit.Application.Profiles;
using MicroRabbit.Domain.Core.Dtos;
using MicroRabbit.Orders.Application.Dtos.OrderItems;
using MicroRabbit.Orders.Application.Dtos.Orders;
using MicroRabbit.Orders.Domain.Models;

namespace MicroRabbit.Orders.Application.Profiles
{
    public class OrdersProfile : BaseProfile<Order, OrderResponse, AddOrderRequest, UpdateOrderRequest>
    {
        public OrdersProfile()
        {
            //OrdersController Service.AddAsync in addition to BaseProfile: CreatedDate + LastUpdatedDate
            CreateMap<AddOrderRequest, Order>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.LastUpdatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => OrderStatus.Created));

            //OrdersController CreateEvent
            CreateMap<OrderResponse, CommonOrder>();
            CreateMap<OrderItemResponse, CommonOrderedBook>()
                .ForMember(dest => dest.OrderItemId, opt => opt.MapFrom(src => src.Id));

            //.ReverseMap();
        }
    }
}
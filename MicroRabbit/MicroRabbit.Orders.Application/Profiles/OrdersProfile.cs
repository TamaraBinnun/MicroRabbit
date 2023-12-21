using AutoMapper;
using MicroRabbit.Orders.Application.Dtos.Orders;
using MicroRabbit.Orders.Domain.Models;

namespace MicroRabbit.Orders.Application.Profiles
{
    public class OrdersProfile : Profile
    {
        public OrdersProfile()
        {
            //OrdersController Service.GetAllAsync
            CreateMap<Order, OrderResponse>();

            //OrdersController Service.AddAsync
            CreateMap<AddOrderRequest, Order>()
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => OrderStatus.Created))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.LastUpdatedDate, opt => opt.MapFrom(src => DateTime.Now));

            //OrdersController Service.UpdateAsync
            CreateMap<UpdateOrderRequest, Order>()
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => OrderStatus.Updated))
                .ForMember(dest => dest.LastUpdatedDate, opt => opt.MapFrom(src => DateTime.Now));

            //.ReverseMap();
        }
    }
}
using AutoMapper;
using MicroRabbit.Orders.Application.Dtos.Orders;
using MicroRabbit.Orders.Domain.Models;

namespace MicroRabbit.Orders.Application.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderResponse>();
            CreateMap<AddOrderRequest, Order>();
            CreateMap<UpdateOrderRequest, Order>();
            //.ReverseMap();
        }
    }
}
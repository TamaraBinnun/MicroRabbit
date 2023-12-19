using AutoMapper;
using MicroRabbit.Domain.Core.Dtos;
using MicroRabbit.Orders.Application.Dtos.OrderItems;
using MicroRabbit.Orders.Domain.Models;

namespace MicroRabbit.Orders.Application.Profiles
{
    public class OrderItemsProfile : Profile
    {
        public OrderItemsProfile()
        {
            //OrdersController Service.GetAllAsync
            CreateMap<OrderItem, OrderItemResponse>();

            //OrdersController Service.AddAsync
            CreateMap<AddOrderItemRequest, OrderItem>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.LastUpdatedDate, opt => opt.MapFrom(src => DateTime.Now));

            //OrdersController Service.UpdateAsync
            CreateMap<UpdateOrderItemRequest, OrderItem>()
                .ForMember(dest => dest.LastUpdatedDate, opt => opt.MapFrom(src => DateTime.Now));

            //OrdersController OrderBooksService.AddAsync
            CreateMap<OrderItemResponse, CommonOrderedBook>();

            //OrdersController OrderBooksService.HandleUpdateOrderItems
            CreateMap<UpdateOrderItemRequest, CommonOrderedBook>();

            //.ReverseMap();
        }
    }
}
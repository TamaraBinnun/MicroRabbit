using AutoMapper;
using MicroRabbit.Domain.Core.Dtos;
using MicroRabbit.Orders.Application.Dtos;
using MicroRabbit.Orders.Application.Dtos.OrderItems;
using MicroRabbit.Orders.Domain.Models;

namespace MicroRabbit.Orders.Application.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookResponse>();
            CreateMap<OrderItemResponse, BookUnits>();
            //.ReverseMap();
        }
    }
}
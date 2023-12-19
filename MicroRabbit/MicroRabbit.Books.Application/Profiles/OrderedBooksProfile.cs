using AutoMapper;
using MicroRabbit.Books.Application.Dtos.OrderedBooks;
using MicroRabbit.Books.Domain.Models;
using MicroRabbit.Domain.Core.Dtos;

namespace MicroRabbit.Books.Application.Profiles
{
    public class OrderedBooksProfile : Profile
    {
        public OrderedBooksProfile()
        {
            //OrderedBooksController Service.GetAllAsync
            CreateMap<OrderedBook, OrderedBookResponse>();

            //OrderedBooksController Service.AddAsync
            CreateMap<AddOrderedBookRequest, OrderedBook>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.LastUpdatedDate, opt => opt.MapFrom(src => DateTime.Now));

            //OrderedBooksController Service.UpdateAsync
            CreateMap<UpdateOrderedBookRequest, OrderedBook>()
                .ForMember(dest => dest.LastUpdatedDate, opt => opt.MapFrom(src => DateTime.Now));

            //OrderedBooksController OrderedBooksService.UseEventToUpdateOrderedBooksAsync
            CreateMap<CommonOrderedBook, AddOrderedBookRequest>();

            //OrderedBooksController OrderedBooksService.UseEventToUpdateOrderedBooksAsync
            CreateMap<CommonOrderedBook, UpdateOrderedBookRequest>();

            //.ReverseMap();
        }
    }
}
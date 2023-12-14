using AutoMapper;
using MicroRabbit.Books.Application.Dtos;
using MicroRabbit.Books.Domain.Models;
using MicroRabbit.Domain.Core.Dtos;

namespace MicroRabbit.Books.Application.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookResponse>();
            CreateMap<AddBookRequest, Book>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<UpdateBookRequest, Book>()
                .ForMember(dest => dest.LastUpdatedDate, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<StockResponse, BookUnits>().ReverseMap();
        }
    }
}
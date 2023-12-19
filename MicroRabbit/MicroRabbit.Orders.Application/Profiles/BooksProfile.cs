using AutoMapper;
using MicroRabbit.Domain.Core.Dtos;
using MicroRabbit.Orders.Application.Dtos;
using MicroRabbit.Orders.Application.Dtos.Books;
using MicroRabbit.Orders.Domain.Models;

namespace MicroRabbit.Orders.Application.Profiles
{
    public class BooksProfile : Profile
    {
        public BooksProfile()
        {
            //BooksController Service.GetAllAsync
            CreateMap<Book, BookResponse>();

            //BooksController Service.AddAsync
            CreateMap<AddBookRequest, Book>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.LastUpdatedDate, opt => opt.MapFrom(src => DateTime.Now));

            //BooksController Service.UpdateAsync
            CreateMap<UpdateBookRequest, Book>()
                .ForMember(dest => dest.LastUpdatedDate, opt => opt.MapFrom(src => DateTime.Now));

            //BooksController BooksService.UseEventToUpdateBookAsync
            CreateMap<CommonBookData, AddBookRequest>()
               .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.BookId));

            //BooksController BooksService.UseEventToUpdateBookAsync
            CreateMap<CommonBookData, UpdateBookRequest>()
               .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.BookId));

            //.ReverseMap();
        }
    }
}
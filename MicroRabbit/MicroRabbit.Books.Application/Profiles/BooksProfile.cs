using AutoMapper;
using MicroRabbit.Books.Application.Dtos.Books;
using MicroRabbit.Books.Domain.Models;
using MicroRabbit.Domain.Core.Dtos;

namespace MicroRabbit.Books.Application.Profiles
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

            //BooksController.PostAsync
            CreateMap<BookResponse, CommonBookData>()
                 .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.Id));

            //BooksController.PutAsync
            CreateMap<UpdateBookRequest, CommonBookData>()
                 .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
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
            CreateMap<AddBookRequest, Book>();//.ForMember(x => x.CreatedDate = DateTime.Now);
            CreateMap<UpdateBookRequest, Book>();//.ForMember(x => x.LastUpdatedDate = DateTime.Now);

            CreateMap<StockResponse, BookUnits>().ReverseMap();
        }
    }
}
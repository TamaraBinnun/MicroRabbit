using MicroRabbit.Application.Profiles;
using MicroRabbit.Domain.Core.Dtos;
using MicroRabbit.Orders.Application.Dtos;
using MicroRabbit.Orders.Application.Dtos.Books;
using MicroRabbit.Orders.Domain.Models;

namespace MicroRabbit.Orders.Application.Profiles
{
    public class BooksProfile : BaseProfile<Book, BookResponse, AddBookRequest, UpdateBookRequest>
    {
        public BooksProfile()
        {
            //BooksController BooksService UseEventToUpdateBookAsync
            CreateMap<CommonBookData, AddBookRequest>()
               .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.BookId));

            //.ReverseMap();
        }
    }
}
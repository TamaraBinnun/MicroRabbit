using MicroRabbit.Application.Profiles;
using MicroRabbit.Books.Application.Dtos.Books;
using MicroRabbit.Books.Domain.Models;
using MicroRabbit.Domain.Core.Dtos;

namespace MicroRabbit.Books.Application.Profiles
{
    public class BooksProfile : BaseProfile<Book, BookResponse, AddBookRequest, UpdateBookRequest>
    {
        public BooksProfile()
        {
            //BooksController DeleteAsync
            CreateMap<(BookResponse bookResponse, bool IsDeleted), CommonBookData>()
                .IncludeMembers(s => s.bookResponse)
               .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted));

            //BooksController.PostAsync + PutAsync + DeleteAsync
            CreateMap<BookResponse, CommonBookData>()
                 .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
using MicroRabbit.Application.Profiles;
using MicroRabbit.Books.Application.Dtos.OrderedBooks;
using MicroRabbit.Books.Domain.Models;
using MicroRabbit.Domain.Core.Dtos;

namespace MicroRabbit.Books.Application.Profiles
{
    public class OrderedBooksProfile : BaseProfile<OrderedBook, OrderedBookResponse, AddOrderedBookRequest, UpdateOrderedBookRequest>
    {
        public OrderedBooksProfile()
        {
            //OrderedBooksController => OrderedBooksService => GetAddedItems
            CreateMap<CommonOrderedBook, AddOrderedBookRequest>()
                 .ForMember(dest => dest.ExternalBookId, opt => opt.MapFrom(src => src.BookId));

            //OrderedBooksController => OrderedBooksService => GetUpdatedItems
            CreateMap<CommonOrderedBook, UpdateOrderedBookRequest>();

            //.ReverseMap();
        }
    }
}
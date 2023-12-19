using MicroRabbit.Domain.Core.Dtos;

namespace MicroRabbit.Domain.Core.Interfaces
{
    public interface ISynchronousSender
    {
        //sender:book microservice
        //receiver: order microservice
        //after creating a new book (or update/delete book data) used for update book external id and title
        //UpdateDataAsync<BookData>(entity,  _config["MicroRabbitOrders:BooksApi"])

        //sender:order microservice
        //receiver: book microservice
        //after making a new order (or update/delete order) used for update units remaining for ordered books
        //UpdateDataAsync<IEnumerable<BookUnits>>(entities,_config["MicroRabbitBooks:OrderedBooksApi"])

        Task<bool> UpdateDataAsync<T>(T entity, string url) where T : class;
    }
}
using AutoMapper;
using MicroRabbit.Application.Services;
using MicroRabbit.Books.Application.Dtos.Books;
using MicroRabbit.Books.Application.Interfaces;
using MicroRabbit.Books.Domain.Commands;
using MicroRabbit.Books.Domain.Interfaces;
using MicroRabbit.Books.Domain.Models;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Dtos;
using MicroRabbit.Domain.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace MicroRabbit.Books.Application.Services
{
    public class BooksService : Service<Book, BookResponse, AddBookRequest, UpdateBookRequest>,
                                IBooksService
    {
        private readonly IBooksRepository<UpdateBookRequest> _repository;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;
        private readonly ISynchronousSender _synchronousSender;
        private readonly IConfiguration _config;

        public BooksService(IBooksRepository<UpdateBookRequest> repository,
                           IEventBus eventBus,
                           IMapper mapper,
                            ISynchronousSender synchronousSender,
                            IConfiguration config)
            : base(repository, eventBus, mapper)
        {
            _repository = repository;
            _eventBus = eventBus;
            _mapper = mapper;
            _synchronousSender = synchronousSender;
            _config = config;
        }

        public async Task<bool> CreateEventToUpdateBookAsync(CommonBookData bookData)
        {
            if (bookData == null)
            {
                return false;
            }

            var updateBookDataCommand = new UpdateBookCommand(bookData);

            var response = await _eventBus.SendCommand(updateBookDataCommand);

            if (!response)
            {//if rabbitmq not available then send by http
                Console.WriteLine($"Sending by rabbitmq has failed.");
                response = await _synchronousSender.UpdateDataAsync<CommonBookData>(bookData, _config["MicroRabbitOrders:BooksApi"]!);
                Console.WriteLine($"Sent directly by synchronous sender. success: {response}");
            }
            return response;
        }
    }
}
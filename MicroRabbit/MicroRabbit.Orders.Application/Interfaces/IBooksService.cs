﻿using MicroRabbit.Application.Interfaces;
using MicroRabbit.Domain.Core.Dtos;
using MicroRabbit.Orders.Application.Dtos;
using MicroRabbit.Orders.Application.Dtos.Books;
using MicroRabbit.Orders.Domain.Models;

namespace MicroRabbit.Orders.Application.Interfaces
{
    public interface IBooksService : IService<BookResponse, AddBookRequest, UpdateBookRequest>
    {
        Task UseEventToUpdateBookAsync(CommonBookData commonBookData);
    }
}
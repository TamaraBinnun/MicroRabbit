﻿using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Transfer.Domain.Events;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Domain.EventHandles
{
    public class EventToCreateTransferHandler : IEventHandler<EventToCreateTransfer>
    {
        private readonly ITransferRepository _transferRepository;

        public EventToCreateTransferHandler(ITransferRepository transferRepository)
        {
            _transferRepository = transferRepository;
        }

        public Task Handle(EventToCreateTransfer @event)
        {
            _transferRepository.Add(new AccountTransfer
            {
                FromAccount = @event.From,
                ToAccount = @event.To,
                TransferAmount = @event.Amount
            });
            return Task.CompletedTask;
        }
    }
}
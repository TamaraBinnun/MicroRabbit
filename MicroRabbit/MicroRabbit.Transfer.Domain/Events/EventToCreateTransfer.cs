﻿using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Transfer.Domain.Events
{
    public class EventToCreateTransfer : Event
    {
        public EventToCreateTransfer(int from, int to, decimal amount)
        {
            From = from;
            To = to;
            Amount = amount;
        }

        public int From { get; private set; }
        public int To { get; private set; }
        public decimal Amount { get; private set; }
    }
}
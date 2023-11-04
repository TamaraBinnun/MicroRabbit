﻿using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Application.Interfaces
{
    public interface IAccountTransferService
    {
        IEnumerable<AccountTransfer> GetAccountTransfers();
    }
}
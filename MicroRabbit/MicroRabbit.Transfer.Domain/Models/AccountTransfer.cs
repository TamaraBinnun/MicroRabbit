﻿namespace MicroRabbit.Transfer.Domain.Models
{
    public class AccountTransfer
    {
        public int Id { get; set; }

        public int FromAccount { get; set; }

        public int ToAccount { get; set; }

        public decimal TransferAmount { get; set; }
    }
}
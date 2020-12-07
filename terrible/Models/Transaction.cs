﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace terrible.Models
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public int SenderID { get; set; }
        public int ReceiverID { get; set; }
        public decimal TransferAmount { get; set; }

        public DateTime TransactionTime { get; set; }
    }
}

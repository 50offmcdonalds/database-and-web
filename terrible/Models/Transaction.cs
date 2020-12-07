using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace terrible.Models
{
    public class Transaction
    {
        public int SenderID { get; set; }
        public int ReceiverID { get; set; }
        public int TransferAmount { get; set; }
    }
}

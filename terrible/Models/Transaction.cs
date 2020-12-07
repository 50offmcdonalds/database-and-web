using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace terrible.Models
{
    public class Transaction
    {
        [Display (Name = "Transaction ID")]
        public int TransactionID { get; set; }
        [Display (Name = "Sender")]
        public int SenderID { get; set; }
        [Display (Name = "Receiver")]
        public int ReceiverID { get; set; }
        [Display (Name = "Amount")]
        public decimal TransferAmount { get; set; }
        [Display (Name = "Date")]
        public DateTime TransactionTime { get; set; }
    }
}

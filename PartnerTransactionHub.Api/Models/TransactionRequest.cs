using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTSCo.Models
{
    public class TransactionRequest
    {
        public string PartnerId { get; set; }
        public string TransactionReference { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

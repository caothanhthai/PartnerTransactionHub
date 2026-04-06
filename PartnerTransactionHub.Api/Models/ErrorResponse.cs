using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartnerTransactionHub.Api.Models
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public string TraceId { get; set; }
        public string Details { get; set; }
    }
}

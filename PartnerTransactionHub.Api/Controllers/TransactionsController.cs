using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTSCo.Models;
using TTSCo.Services;
using TTSCom.Messaging;

namespace TTSCo.Controllers
{
    [ApiController]
    [Route("api/v1/partner/transactions")]
    public class TransactionsController : ControllerBase
    {
        private readonly IPartnerVerificationService _verificationService;
        private readonly IMessagePublisher _publisher;

        public TransactionsController(IPartnerVerificationService verificationService, IMessagePublisher publisher)
        {
            _verificationService = verificationService;
            _publisher = publisher;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionRequest request)
        {
            // Validation automatique via FluentValidation

            var isValidPartner = await _verificationService.VerifyPartnerAsync(request.PartnerId);

            if (!isValidPartner)
            {
                return BadRequest(new
                {
                    message = "Invalid partner"
                });
            }

            await _publisher.PublishAsync(request);

            return Accepted(new
            {
                message = "Transaction accepted and queued",
                reference = request.TransactionReference
            });
        }
    }
}

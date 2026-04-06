using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTSCo.Controllers
{
    [ApiController]
    [Route("mock/partner-verification")]
    public class PartnerVerificationController : ControllerBase
    {
        private static readonly Random _random = new Random();

        [HttpGet("{partnerId}")]
        public IActionResult Verify(string partnerId)
        {
            var chance = _random.Next(100);

            if (chance < 30)
            {
                throw new TimeoutException("Simulated timeout");
            }

            return Ok(new
            {
                partnerId,
                isValid = true
            });
        }
    }
}

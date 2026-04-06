using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TTSCo.Services
{
    public class PartnerVerificationService : IPartnerVerificationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PartnerVerificationService> _logger;

        public PartnerVerificationService(HttpClient httpClient, ILogger<PartnerVerificationService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<bool> VerifyPartnerAsync(string partnerId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"mock/partner-verification/{partnerId}");

                if (!response.IsSuccessStatusCode)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Partner verification failed");
                return false; // fallback graceful
            }
        }
    }
}

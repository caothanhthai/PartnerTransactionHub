using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTSCo.Services
{
    public interface IPartnerVerificationService
    {
        Task<bool> VerifyPartnerAsync(string partnerId);
    }
}

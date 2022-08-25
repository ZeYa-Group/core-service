using ServiceAutomation.Canvas.WebApi.Interfaces;
using System;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class UserReferralService : IUserReferralService
    {
        public string GenerateLinc(Guid userId)
        {
            return $"https://trifecta.com/partnerlink/{userId}";
        }
    }
}

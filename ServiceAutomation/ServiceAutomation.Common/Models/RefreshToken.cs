using System;

namespace ServiceAutomation.Common.Models
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}

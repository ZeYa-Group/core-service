using System;

namespace ServiceAutomation.Common.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}

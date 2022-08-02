using System;

namespace ServiceAutomation.Canvas.WebApi.Models
{
    public class Token
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public DateTime AccessTokenExpiryTime { get; set; }
    }
}

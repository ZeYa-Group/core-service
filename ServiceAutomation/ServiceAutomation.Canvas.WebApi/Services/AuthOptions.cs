using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class AuthOptions
    {
        public const string ISSUER = "AppAuthServer"; 
        public const string AUDIENCE = "AppAuthClient"; 
        private const string KEY = "gaWx9merTgX8kxX80wPpgGW19DpYCpOG";
        public const int ACCESSTOKENLIFETIME = 1;
        public const int REFRESHTOKENLIFETIME = 1;

        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}

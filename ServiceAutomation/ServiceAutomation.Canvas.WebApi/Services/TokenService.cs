using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models;
using ServiceAutomation.Common.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class TokenService : ITokenService
    {
        public Token Create(UserModel user)
        {
            throw new System.NotImplementedException();
        }

        public Token Create(List<Claim> claims)
        {
            throw new System.NotImplementedException();
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            throw new System.NotImplementedException();
        }

        public bool VerifyRefreshToken(RefreshToken user, Token tokenData)
        {
            throw new System.NotImplementedException();
        }
    }
}

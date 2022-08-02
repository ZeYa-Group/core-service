using ServiceAutomation.Canvas.WebApi.Models;
using ServiceAutomation.Common.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace ServiceAutomation.Canvas.WebApi.Interfaces
{
    public interface ITokenService
    {
        Token Create(UserModel user);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        Token Create(List<Claim> claims);
        bool VerifyRefreshToken(RefreshToken user, Token tokenData);
    }
}

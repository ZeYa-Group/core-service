using ServiceAutomation.Canvas.WebApi.Models;
using ServiceAutomation.Common.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Interfaces
{
    public interface ITokenService
    {
        Task<Guid> Create(RefreshToken token);
        Task<RefreshToken> GetRefreshToken(string token);
        Task<RefreshToken> GetRefreshToken(Guid userId);
        Task DeleteRefreshToken(Guid id);
    }
}

﻿using System.Threading.Tasks;
using ServiceAutomation.Canvas.WebApi.Models;
using ServiceAutomation.Canvas.WebApi.Models.RequestsModels;
using ServiceAutomation.Common.Models;

namespace ServiceAutomation.Canvas.WebApi.Interfaces
{
    public interface IAuthProvider
    {
        Token Generate(UserModel user);
        string GenerateRefreshToken();
        Task<AuthenticationResult> Authenticate(LoginRequestModel requestModel);
        Task<AuthenticationResult> Register(RegisterRequestModel requestModel);

    }
}

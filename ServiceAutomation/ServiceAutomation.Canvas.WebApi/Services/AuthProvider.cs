﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models;
using ServiceAutomation.Canvas.WebApi.Models.RequestsModels;
using ServiceAutomation.Common.Models;
using ServiceAutomation.DataAccess.DbContexts;
using ServiceAutomation.DataAccess.Schemas.EntityModels;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class AuthProvider : IAuthProvider
    {
        private readonly IUserManager userManager;
        private readonly IMapper mapper;
        private readonly ITokenService tokenService;

        public AuthProvider(IUserManager userManager, IMapper mapper, ITokenService tokenService)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.tokenService = tokenService;
        }

        public async Task<AuthenticationResult> Authenticate(LoginRequestModel requestModel)
        {
            var user = await userManager.GetByEmail(requestModel.Email);

            if (user == null)
            {
                return new AuthenticationResult()
                {
                    Success = false,
                    Errors = new[] {"Invalid email adress"}
                };
            }

            var isPasswordCorrecrt = VerifyPasswordhash(requestModel.Password, user.PasswordHash, user.PasswordSalt);

            if (!isPasswordCorrecrt)
            {
                return new AuthenticationResult()
                {
                    Success = false,
                    Errors = new[] { "Invalid password" }
                };
            }

            var expiredToken = await tokenService.GetRefreshToken(user.Id);
            await tokenService.DeleteRefreshToken(expiredToken.Id);

            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            await tokenService.Create(new RefreshToken { UserId = user.Id, Token = refreshToken });

            return new AuthenticationResult()
            {
                Success = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        public async Task<AuthenticationResult> Register(RegisterRequestModel requestModel)
        {
            var hashModel = CreatePasswordHash(requestModel.Password);

            var user = mapper.Map<UserModel>(requestModel);

            user.PasswordSalt = hashModel.PasswordSalt;
            user.PasswordHash = hashModel.PasswordHash;

            var responseUser = await userManager.AddUser(user);

            var accessToken = GenerateAccessToken(responseUser);
            var refreshToken = GenerateRefreshToken();

            await tokenService.Create(new RefreshToken { Token = refreshToken, UserId = responseUser.Id });

            return new AuthenticationResult
            {
                Success = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        public async Task<AuthenticationResult> Refresh(RefreshRequestModel requestModel)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            TokenValidationParameters parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = AuthOptions.ISSUER,
                ValidateAudience = true,
                ValidAudience = AuthOptions.AUDIENCE,
                ValidateLifetime = true,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                handler.ValidateToken(requestModel.RefreshToken, parameters, out SecurityToken validatedToken);
            }
            catch (Exception ex)
            {
                return new AuthenticationResult()
                {
                    Success = false,
                    Errors = new[] { $"{ex.Message}" }
                };
            }

            var refreshToken = await tokenService.GetRefreshToken(requestModel.RefreshToken);

            if(refreshToken == null)
            {
                return new AuthenticationResult()
                {
                    Success = false,
                    Errors = new[] { "Invalid refresh token" }
                };
            }

            await tokenService.DeleteRefreshToken(refreshToken.Id);

            var currentUser = await userManager.GetById(refreshToken.UserId);

            if(currentUser == null)
            {
                return new AuthenticationResult()
                {
                    Success = false,
                    Errors = new[] { "User not found" }
                };
            }

            var currentAccessToken = GenerateAccessToken(currentUser);
            var currentRefreshToken = GenerateRefreshToken();

            await tokenService.Create(new RefreshToken { Token = currentRefreshToken, UserId = currentUser.Id });

            return new AuthenticationResult()
            {
                Success = true,
                RefreshToken = currentRefreshToken,
                AccessToken = currentAccessToken
            };
        }

        private string GenerateAccessToken(UserModel user)
        {
            var securityKey = AuthOptions.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var token = new JwtSecurityToken(
                AuthOptions.ISSUER,
                AuthOptions.AUDIENCE,
                claims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private PasswordHashModel CreatePasswordHash(string password)
        {
            using(var hmac = new HMACSHA512())
            {
                return new PasswordHashModel()
                {
                    PasswordSalt = hmac.Key,
                    PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password))
                };
            }
        }

        private bool VerifyPasswordhash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes((string)password));

                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string GenerateRefreshToken()
        {
            var securityKey = AuthOptions.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                AuthOptions.ISSUER,
                AuthOptions.AUDIENCE,
                expires: DateTime.Now.AddMinutes(100),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

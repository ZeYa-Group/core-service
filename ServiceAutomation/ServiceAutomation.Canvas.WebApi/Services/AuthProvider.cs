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

        public AuthProvider(IUserManager userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<AuthenticationResult> Authenticate(LoginRequestModel requestModel)
        {
            //var user = await dbContext.Users.FirstOrDefaultAsync(x =>
            //x.Email == requestModel.Email); //&& x.Password == requestModel.Password);

            //var res = new UserModel()
            //{
            //    Name = user.Name,
            //    Email = user.Email,
            //    //Password = user.Password,
            //    Surname = user.Surname
            //};

            //return res;

            return null;
        }

        public Token Generate(UserModel user)
        {
            var securityKey = AuthOptions.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Name),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var token = new JwtSecurityToken(
                AuthOptions.ISSUER,
                AuthOptions.AUDIENCE,
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new Token
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                AccessTokenExpiryTime = token.ValidTo,
                RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpiryTime = DateTime.UtcNow.Add(TimeSpan.FromMinutes(AuthOptions.REFRESHTOKENLIFETIME))
            };
        }

        public async Task<AuthenticationResult> Register(RegisterRequestModel requestModel)
        {
            var hashModel = CreatePasswordHash(requestModel.Password);

            //var user = new UserModel()
            //{
            //    Name = requestModel.Name,
            //    Surname = requestModel.Surname,
            //    Email = requestModel.Email,
            //    PasswordHash = hashModel.PasswordHash,
            //    PasswordSalt = hashModel.PasswordSalt
            //};

            var user = mapper.Map<UserModel>(requestModel);

            user.PasswordSalt = hashModel.PasswordSalt;
            user.PasswordHash = hashModel.PasswordHash;

            var responseUser = await userManager.AddUser(user);

            var token = Generate(user);

            return new AuthenticationResult
            {
                Success = true,
                Token = token.AccessToken
            };
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

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}

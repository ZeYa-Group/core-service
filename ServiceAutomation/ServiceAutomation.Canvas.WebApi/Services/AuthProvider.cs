using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models;
using ServiceAutomation.Canvas.WebApi.Models.RequestsModels;
using ServiceAutomation.DataAccess.DbContexts;
using ServiceAutomation.DataAccess.DbSets;
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
        private readonly PotgreSqlContext dbContext;
        public AuthProvider(PotgreSqlContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<UserModel> Authenticate(LoginRequestModel requestModel)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(x =>
            x.Email == requestModel.Email); //&& x.Password == requestModel.Password);

            var res = new UserModel()
            {
                Name = user.Name,
                Email = user.Email,
                //Password = user.Password,
                Surname = user.Surname
            };

            return res;
        }

        public string Generate(UserModel user)
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

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<UserModel> Register(RegisterRequestModel requestModel)
        {
            var user = new UserModel();
            var userHashPassword = CreatePasswordHash(requestModel.Password);

            user.Name = requestModel.Name;
            user.Surname = requestModel.Surname;
            user.Email = requestModel.Email;
            user.PasswordHash = userHashPassword.PasswordHash;
            user.PasswordSalt = userHashPassword.PasswordSalt;

            return user;
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
    }
}

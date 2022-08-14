using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServiceAutomaion.Services.Interfaces;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models;
using ServiceAutomation.Common.Models;
using ServiceAutomation.DataAccess.DbContexts;
using ServiceAutomation.DataAccess.Schemas.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly AppDbContext dbContext;
        private readonly IIdentityGenerator generator;
        private readonly IMapper mapper;

        public TokenService(AppDbContext dbContext, IIdentityGenerator generator, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.generator = generator;
            this.mapper = mapper;
        }

        public async Task<Guid> CreateAsync(RefreshToken token)
        {
            var refreshToken = new RefreshTokenEntity()
            {
                Id = generator.Generate(),
                Token = token.Token,
                UserId = token.UserId
            };

            await dbContext.RefresTokens.AddAsync(refreshToken);
            await dbContext.SaveChangesAsync();

            return refreshToken.Id;
        }

        public async Task DeleteRefreshTokenAsync(Guid id)
        {
            var refreshToken = await dbContext.RefresTokens.FirstOrDefaultAsync(tkn => tkn.Id == id);

            if(refreshToken == null)
            {
                return;
            }

            dbContext.RefresTokens.Remove(refreshToken);
            await dbContext.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(string token)
        {
            var tokenModel = await dbContext.RefresTokens.FirstOrDefaultAsync(tkn => tkn.Token == token);

            if(tokenModel == null)
            {
                return null;
            }

            return mapper.Map<RefreshToken>(tokenModel);
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(Guid userId)
        {
            var tokenModel = await dbContext.RefresTokens.FirstOrDefaultAsync(tkn => tkn.UserId == userId);

            if (tokenModel == null)
            {
                return null;
            }

            return mapper.Map<RefreshToken>(tokenModel);
        }
    }
}

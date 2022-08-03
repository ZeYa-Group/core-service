﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServiceAutomaion.Services.Interfaces;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Common.Models;
using ServiceAutomation.DataAccess.DbContexts;
using ServiceAutomation.DataAccess.Schemas.EntityModels;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class UserManager : IUserManager
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IIdentityGenerator identityGenerator;

        public UserManager(AppDbContext dbContext, IMapper mapper, IIdentityGenerator identityGenerator)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.identityGenerator = identityGenerator;
        }

        public async Task<UserModel> AddUser(UserModel user)
        {
            var addedUser = new UserContactEntity()
            {
                Id = identityGenerator.Generate(),
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt,
            };

            await dbContext.UserContacts.AddAsync(addedUser);
            await dbContext.SaveChangesAsync();

            return mapper.Map<UserModel>(addedUser);
        }

        public async Task<UserModel> GetByEmail(string email)
        {
            var user = await dbContext.UserContacts.FirstOrDefaultAsync(x => x.Email == email.ToLower());

            if(user == null)
            {
                return null;
            }

            return mapper.Map<UserModel>(user);
        }

        public async Task<UserModel> GetById(Guid id)
        {
            var user = await dbContext.UserContacts.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return null;
            }

            return mapper.Map<UserModel>(user);
        }

        public async Task<bool> IsUserAlreadyExists(string email)
        {
            var user = await dbContext.UserContacts.Where(x => x.Email == email.ToLower()).ToListAsync();

            if(user.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}

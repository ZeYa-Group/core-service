﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models.ResponseModels;
using ServiceAutomation.DataAccess.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class WithdrawService : IWithdrawService
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;

        public WithdrawService(AppDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<WithdrawResponseModel>> GetWithdrawHistory(Guid userId)
        {
            var user = await dbContext.Users.Where(x => x.Id == userId)
                .Include(x => x.Credentional)
                .ThenInclude(x => x.WithdrawTransactions)
                .FirstOrDefaultAsync();

            var withdrawHistory = user.Credentional.WithdrawTransactions;

            //.SelectMany(x => x.WithdrawTransactions)
            //.ToListAsync();

            var response = withdrawHistory.Select(x => mapper.Map<WithdrawResponseModel>(x));

            return response;
        }
    }
}

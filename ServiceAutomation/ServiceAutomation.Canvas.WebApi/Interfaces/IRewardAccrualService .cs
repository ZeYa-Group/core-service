﻿using System;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Interfaces
{
    public interface IRewardAccrualService
    {
        Task AccrueRewardForBasicLevelAsync(Guid userId);

        Task AccrueRewardForSaleAsync(Guid userId, decimal sellingPrice);
    }
}

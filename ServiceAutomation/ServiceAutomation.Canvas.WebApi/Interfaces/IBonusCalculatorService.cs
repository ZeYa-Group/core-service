﻿using System.Threading.Tasks;
using System;

namespace ServiceAutomation.Canvas.WebApi.Interfaces
{
    public interface IBonusCalculatorService
    {
        Task<int> CalculateBonusAsync(Guid userId);
    }
}

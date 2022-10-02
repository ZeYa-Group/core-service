﻿using ServiceAutomation.DataAccess.Models.EntityModels;
using ServiceAutomation.DataAccess.Models.Enums;

namespace ServiceAutomation.DataAccess.DataSeeding
{
    internal static class LevelBonusRewardsInitialData
    {
        public static LevelBonusRewardEntity[] LevelBonusRewardSeeds = new LevelBonusRewardEntity[]
        {
            new LevelBonusRewardEntity {LevelId = BasicLevelsEnitialData.BasicLevelIdsByType[Level.ThirdLevel], Reward = 750 },
            new LevelBonusRewardEntity {LevelId = BasicLevelsEnitialData.BasicLevelIdsByType[Level.FourthLevel], Reward = 1300 },
            new LevelBonusRewardEntity {LevelId = BasicLevelsEnitialData.BasicLevelIdsByType[Level.FifthLevel], Reward = 2500 },
            new LevelBonusRewardEntity {LevelId = BasicLevelsEnitialData.BasicLevelIdsByType[Level.SixthLevel], Reward = 4000 },
            new LevelBonusRewardEntity {LevelId = BasicLevelsEnitialData.BasicLevelIdsByType[Level.SeventhLevel], Reward = 7000 },
            new LevelBonusRewardEntity {LevelId = BasicLevelsEnitialData.BasicLevelIdsByType[Level.EighthLevel], Reward = 12000 },
            new LevelBonusRewardEntity {LevelId = BasicLevelsEnitialData.BasicLevelIdsByType[Level.NinthLevel], Reward = 25000 },
            new LevelBonusRewardEntity {LevelId = BasicLevelsEnitialData.BasicLevelIdsByType[Level.TenthLevel], Reward = 50000 }
        };
    }
}

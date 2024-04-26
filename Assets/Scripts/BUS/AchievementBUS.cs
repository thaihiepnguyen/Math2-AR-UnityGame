using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

public class AchievementBUS
{
    private readonly AchievementDAO _achievementDAO = new();

    public async Task<BaseDTO<List<AchievementDTO>>> GetAchievementsByUserId()
    {
        return await _achievementDAO.GetAchievementsByUserId();
    }

    public async Task<BaseDTO<object>> GetReward(RewardDTO rewardDTO) {
        return await _achievementDAO.GetReward(rewardDTO);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class AchievementBUS
{
    private readonly AchievementDAO _achievementDAO = new();

    public async Task<BaseDTO<List<AchievementDTO>>> GetAchievementsByUserId()
    {
        return await _achievementDAO.GetAchievementsByUserId();
    }
}

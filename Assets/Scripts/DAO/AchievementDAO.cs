using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class AchievementDAO
{
    public async Task<BaseDTO<List<AchievementDTO>>> GetAchievementsByUserId()
    {
        return await API.Get<List<AchievementDTO>>($"{GlobalVariable.server_url}/achievements/get-by-user-id");
    }

    public async Task<BaseDTO<object>> GetReward(RewardDTO rewardDTO) {
        return await API.Post<RewardDTO, object>($"{GlobalVariable.server_url}/achievements/get-reward", rewardDTO);
    }
}

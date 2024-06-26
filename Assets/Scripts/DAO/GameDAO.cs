using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class GameDAO
{
    public async Task<BaseDTO<GameDTO>> GetGameDataByLessonId(int lessonId)
    {
        return await API.Get<GameDTO>($"{GlobalVariable.server_url}/game/get-game-by-lesson-id/{lessonId}");
    }
}
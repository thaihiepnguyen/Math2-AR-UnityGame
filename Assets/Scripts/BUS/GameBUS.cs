
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
public class GameBUS
{
    private readonly GameDAO _gameDao = new();

    public async Task<BaseDTO<GameDTO>> GetGameDataByLessonId(int lessonId)
    {
        return await _gameDao.GetGameDataByLessonId(lessonId);
    }
}
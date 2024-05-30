
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
public class LessonBUS
{
    private readonly LessonDAO _lessonDao = new(); 
    
    public async Task<BaseDTO<List<ChapterResponseDTO>>> GetChapterBySemester (int semester)
    {
        return await _lessonDao.GetChapterBySemester(semester);
    }

    public async  Task<BaseDTO<List<LessonDTO>>> GetLessonByChapterId(ChapterDTO chapterDto){
        return await _lessonDao.GetLessonByChapterId(chapterDto);
    }
    public async Task<BaseDTO<LessonDTO>> GetVideoByLessonId(int lessonId)
    {
        return await _lessonDao.GetVideoByLessonId(lessonId);
    }
}
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class LessonDAO
{
    public async Task<BaseDTO<List<ChapterResponseDTO>>> GetChapterBySemester(int semester)
    {
       return await API.Get<List<ChapterResponseDTO>>($"{GlobalVariable.server_url}/lessons/get-chapter-by-semester/{semester}");

    }

    public async Task<BaseDTO<List<LessonDTO>>> GetLessonByChapterId(ChapterDTO chapterDto)
    {
        return await API.Post<ChapterDTO, List<LessonDTO>>($"{GlobalVariable.server_url}/lessons/get-lessons-by-chapter-id", chapterDto);
    }
}
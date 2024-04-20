using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class LessonDAO
{
    public async Task<List<LessonDTO>> GetChapterBySemester(int semester)
    {
       

         try
        {
            var a =  await API.getMethod($"{GlobalVariable.server_url}/lessons/get-chapter-by-semester/{semester}");
            var lessonResponse = JsonConvert.DeserializeObject<BaseDTO<List<LessonDTO>>>(a);

            if (lessonResponse.data != null)
            {
                return lessonResponse.data;
            }
            return null;
        }
        catch(Exception ex) {
            throw new Exception("Error: ", ex);
        }    
    }

    public async Task<BaseDTO<List<LessonDTO>>> GetLessonByChapterId(ChapterDTO chapterDto)
    {
        return await API.Post<ChapterDTO, List<LessonDTO>>($"{GlobalVariable.server_url}/lessons/get-lessons-by-chapter-id", chapterDto);
    }
}
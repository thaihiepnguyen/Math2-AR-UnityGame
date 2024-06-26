using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LessonManager : MonoBehaviour
{
    public static LessonManager instance;

    public static LessonManager GetInstance(){
        return instance;
    }

    private List<ChapterResponseDTO> chapters;
    private int semester = 1;
    private string chapterId = "";

    public string GetChapterId(){
        return chapterId;
    }
    public int GetSemester(){
        return semester;
    }
    public List<ChapterResponseDTO> GetChapters(){
        return chapters;
    }

    private string lessonName ="";
    public string GetLessonName(){
        return lessonName;
    }
    private List<LessonDTO> lessons;
    public List<LessonDTO> GetLessons(){
        return lessons;
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public async void OnClickFirstSemesterButton(){
        semester = 1;
        var lessonBus = new LessonBUS();
        var dt = await lessonBus.GetChapterBySemester(1);
        if (dt.data != null){
            chapters = dt.data;
            SceneManager.LoadScene("ChapterList");
        }  
    }    
    
    public async void OnClickSecondSemesterButton(){
        semester = 2;
        var lessonBus = new LessonBUS();
        var dt = await lessonBus.GetChapterBySemester(2);
        if (dt.data != null){
            chapters = dt.data;
            SceneManager.LoadScene("ChapterList");
        }
    }

    public void Chapter(int index){
        
       chapterId = chapters[index].name;
       SceneManager.LoadScene("ChapterScene");
    }

    public void Lesson(int index){
        lessonName = lessons[index].name;
        SceneManager.LoadScene("LessonScene");
    }

    public async void OnClickChapterLearningButton(){
       var lessonBus = new LessonBUS();
       var response = await lessonBus.GetLessonByChapterId(new ChapterDTO
        {
           chapter = chapterId
        });

         if (response.data !=null)
        {
            lessons = response.data;
          
            SceneManager.LoadScene("LessonList");
         
        }
        else
        {
            Debug.Log($"Error: {response.message}");
           
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LessonList : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI chapterId;
    public GameObject lessonPrefab;
    private List<LessonDTO> lessons;

    static protected string lessonName = "";

    public static string GetLessonName(){
        return lessonName;
    }

    public bool isDone = false;

    void Start()
    {   

        // var title = LessonManager.GetInstance().GetChapterId();

        var title = ChapterList.GetChapterId();
        chapterId.text = String.Format("CHƯƠNG {0}",title[0]);
       
        // var chapters = LessonManager.GetInstance().GetLessons();
        GetData();

       
    }

    void Update(){
         if (lessons!=null){

         TextMeshProUGUI lessonText = lessonPrefab.GetComponentInChildren<TextMeshProUGUI>();
         lessonText.text = lessons[0].name;
        

         for (int i = 0; i < lessons.Count; i++){

            int index = i;
             if (i == 0)
             {
                 Button getStart = lessonPrefab.GetComponentInChildren<Button>();
                    if (getStart!=null)
                    {     
                             getStart.onClick.AddListener(() => {
                             
                                LessonLearning(index);
                    });

                    }
                
                 continue;
             }
            
             GameObject newLesson = Instantiate(lessonPrefab);
                 TextMeshProUGUI newLessonText = newLesson.GetComponentInChildren<TextMeshProUGUI>();

                  Button newGetStart = newLesson.GetComponentInChildren<Button>();

                  

                if (newLessonText != null)
                {
                    newLessonText.text = lessons[i].name;// Set button text 

                }
                if (newGetStart!=null){
                  
                    newGetStart.onClick.AddListener(() => LessonLearning(index));
                }

              
                newLesson.transform.SetParent(lessonPrefab.transform.parent.transform, false);

                isDone = true;

         }
        }
    }
    async void GetData(){
        var lessonBus = new LessonBUS();
       var response = await lessonBus.GetLessonByChapterId(new ChapterDTO
        {
           chapter = ChapterList.GetChapterId()
        });

         if (response.data !=null)
        {
            lessons = response.data;
            Debug.Log(lessons[0].name);
        }
    }

    public void LessonLearning(int index){
       
        // LessonManager.GetInstance().Lesson(index);

         lessonName = lessons[index].name;
        SceneManager.LoadScene("LessonScene");
    }
      
        
    
}

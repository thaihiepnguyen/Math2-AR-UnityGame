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
 
    void Start()
    {   

        var title = LessonManager.GetInstance().GetChapterId();
        chapterId.text = String.Format("CHƯƠNG {0}",title[0]);
       
        var chapters = LessonManager.GetInstance().GetLessons();
         TextMeshProUGUI lessonText = lessonPrefab.GetComponentInChildren<TextMeshProUGUI>();
         lessonText.text = chapters[0].name;
        

         for (int i = 0; i < chapters.Count; i++){

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
                    newLessonText.text = chapters[i].name;// Set button text 

                }
                if (newGetStart!=null){
                  
                    newGetStart.onClick.AddListener(() => LessonLearning(index));
                }

              
                newLesson.transform.SetParent(lessonPrefab.transform.parent.transform, false);
         }
    }


    public void LessonLearning(int index){
         Debug.Log(index);
        LessonManager.GetInstance().Lesson(index);
    }
      
        
    
}

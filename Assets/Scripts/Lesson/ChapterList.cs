using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class ChapterList : MonoBehaviour
{

  
    [SerializeField] private TextMeshProUGUI first;
    [SerializeField] private TextMeshProUGUI second;
    [SerializeField] private TextMeshProUGUI third;

    [SerializeField] private TextMeshProUGUI title;

    void Start()
    {
        title.text =String.Format("HỌC KÌ {0}", LessonManager.GetInstance().GetSemester());
         first.text = String.Format("Chương {0}", LessonManager.GetInstance().GetChapters()[0].chapter.ToCharArray()[0]);
          second.text = String.Format("Chương {0}", LessonManager.GetInstance().GetChapters()[1].chapter.ToCharArray()[0]);
           third.text = String.Format("Chương {0}", LessonManager.GetInstance().GetChapters()[2].chapter.ToCharArray()[0]);
    }

     
    // Update is called once per frame
    void Update()
    {
        
    }

     public void FirstChapter(){
        
       LessonManager.GetInstance().Chapter(0);
     
    }

     public void SecondChapter(){
      LessonManager.GetInstance().Chapter(1);
    }

     public void ThirdChapter(){
     LessonManager.GetInstance().Chapter(2);
    }


}

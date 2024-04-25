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

    
    private List<ChapterResponseDTO> chapters;

    static protected string chapterId=  "";
    public static string GetChapterId(){
      return chapterId;
    }

    // void Awake(){
    //   GetData();
    // }
    async void Start()
    {
        var lessonBus = new LessonBUS();
        var data = await lessonBus.GetChapterBySemester(Semester.GetSemester());
        chapters = data.data;
        // GetData();
        // title.text =String.Format("HỌC KÌ {0}", LessonManager.GetInstance().GetSemester());
        //  first.text = String.Format("Chương {0}", LessonManager.GetInstance().GetChapters()[0].name.ToCharArray()[0]);
        //   second.text = String.Format("Chương {0}", LessonManager.GetInstance().GetChapters()[1].name.ToCharArray()[0]);
        //    third.text = String.Format("Chương {0}", LessonManager.GetInstance().GetChapters()[2].name.ToCharArray()[0]);

        if (chapters != null)
        {
            first.text = String.Format("Chương {0}", chapters[0].name.ToCharArray()[0]);
            second.text = String.Format("Chương {0}", chapters[1].name.ToCharArray()[0]);
            third.text = String.Format("Chương {0}", chapters[2].name.ToCharArray()[0]);
        }
        title.text =String.Format("HỌC KÌ {0}", Semester.GetSemester());

        
    }


    void Update(){
     //  if (chapters!=null){
      //   first.text = String.Format("Chương {0}", chapters[0].name.ToCharArray()[0]);
     //     second.text = String.Format("Chương {0}", chapters[1].name.ToCharArray()[0]);
     //      third.text = String.Format("Chương {0}", chapters[2].name.ToCharArray()[0]);
     //   }
    }


    async void GetData(){
       var lessonBus = new LessonBUS();
        var dt = await lessonBus.GetChapterBySemester(Semester.GetSemester());
        if (dt.data != null){
            chapters = dt.data;
            Debug.Log(chapters[0].name);
        }
    }
     public void FirstChapter(){


      chapterId = chapters[0].name;
       SceneHistory.GetInstance().LoadScene("ChapterScene");
      //  LessonManager.GetInstance().Chapter(0);
     
    }

     public void SecondChapter(){
      chapterId = chapters[1].name;
       SceneHistory.GetInstance().LoadScene("ChapterScene");
      // LessonManager.GetInstance().Chapter(1);
    }

     public void ThirdChapter(){
      chapterId = chapters[2].name;
       SceneHistory.GetInstance().LoadScene("ChapterScene");
    //  LessonManager.GetInstance().Chapter(2);
    }


}

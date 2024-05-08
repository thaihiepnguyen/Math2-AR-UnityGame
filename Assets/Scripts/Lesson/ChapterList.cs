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

    static protected char chapterId = '1';
    static protected string chapterName = "";
    public static char GetChapterId()
    {
        return chapterId;
    }
    public static string GetChapterName()
    {
        return chapterName;
    }

    async void Start()
    {
        var lessonBus = new LessonBUS();
        var data = await lessonBus.GetChapterBySemester(Semester.GetSemester());

        chapters = data.data;

        if (chapters != null)
        {
            first.text = String.Format("Chương {0}", chapters[0].name.ToCharArray()[0]);
            second.text = String.Format("Chương {0}", chapters[1].name.ToCharArray()[0]);
            third.text = String.Format("Chương {0}", chapters[2].name.ToCharArray()[0]);
        }
        title.text = String.Format("HỌC KÌ {0}", Semester.GetSemester());


    }


    void Update()
    {

    }


    async void GetData()
    {
        var lessonBus = new LessonBUS();
        var dt = await lessonBus.GetChapterBySemester(Semester.GetSemester());
        if (dt.data != null)
        {
            chapters = dt.data;
        }
    }
    public void FirstChapter()
    {
        chapterName = chapters[0].name;
        chapterId = chapters[0].name[0];
        SceneHistory.GetInstance().LoadScene("ChapterScene");

    }

    public void SecondChapter()
    {
        chapterName = chapters[1].name;
        chapterId = chapters[1].name[0];
        SceneHistory.GetInstance().LoadScene("ChapterScene");
    }

    public void ThirdChapter()
    {
        chapterName = chapters[2].name;
        chapterId = chapters[2].name[0];
        SceneHistory.GetInstance().LoadScene("ChapterScene");
    }

    public void onReview()
    {
        SceneHistory.GetInstance().LoadScene("ExamListScene");
    }
}

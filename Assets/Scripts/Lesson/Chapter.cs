using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chapter : MonoBehaviour
{
    // public static string title = "";
    [SerializeField] private TextMeshProUGUI chapterId;
    [SerializeField] private TextMeshProUGUI chapterName;
    void Start()
    {
        // var title = LessonManager.GetInstance().GetChapterId();

        var title = ChapterList.GetChapterName();
        chapterId.text = String.Format("CHƯƠNG {0}", title);
        chapterName.text = title.Substring(3, title.Length - 3);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Learning()
    {
        //    LessonList.chapterId = title;
        SceneHistory.GetInstance().LoadScene("LessonList");
        // LessonManager.GetInstance().OnClickChapterLearningButton();
    }
    public void OnClickDoingExercise()
    {
        SceneHistory.GetInstance().LoadScene(GlobalVariable.EXERCISES_SCENE);
    }
}

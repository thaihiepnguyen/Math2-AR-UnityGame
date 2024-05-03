using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Semester : MonoBehaviour
{

    static protected int semester = 0;
    public static int GetSemester(){
        return semester;
    }
    public void FirstSemester(){
        semester = 1;
       SceneHistory.GetInstance().LoadScene("ChapterList");
    }

     public void SecondSemester(){
        semester = 2;
        SceneHistory.GetInstance().LoadScene("ChapterList");
    }

     public void OnClickAchievementButton(){
        SceneHistory.GetInstance().LoadScene("Achievement");
    }
}

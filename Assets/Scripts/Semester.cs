using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Semester : MonoBehaviour
{
    public void FirstSemester(){
       ChapterList.semester = 1;
       SceneManager.LoadScene("ChapterList");
    }

     public void SecondSemester(){
       ChapterList.semester = 2;
        SceneManager.LoadScene("ChapterList");
    }
}

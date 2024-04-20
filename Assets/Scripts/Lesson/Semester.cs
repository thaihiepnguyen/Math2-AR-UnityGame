using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Semester : MonoBehaviour
{
    public void FirstSemester(){
   
       SceneManager.LoadScene("ChapterList");
    }

     public void SecondSemester(){
     
        SceneManager.LoadScene("ChapterList");
    }
}

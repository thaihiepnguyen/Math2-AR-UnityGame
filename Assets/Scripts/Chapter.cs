using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chapter : MonoBehaviour
{
    public static string title = "";
     [SerializeField] private TextMeshProUGUI chapterId;
    [SerializeField] private TextMeshProUGUI chapterName;
    void Start()
    {
        
        chapterId.text = String.Format("CHƯƠNG {0}",title[0]);
        chapterName.text = title.Substring(3,title.Length-3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     public void Learning(){
       LessonList.chapterId = (int) title[0];
       SceneManager.LoadScene("LessonList");
    }



}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lesson : MonoBehaviour
{
     [SerializeField] private TextMeshProUGUI lessonId;
    void Start()
    {
        // lessonId.text = LessonManager.GetInstance().GetLessonName();
        lessonId.text = LessonList.GetLessonName();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

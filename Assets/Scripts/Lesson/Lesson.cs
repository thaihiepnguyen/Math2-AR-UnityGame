using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lesson : MonoBehaviour
{
    LessonBUS lessonBUS = new LessonBUS();
    [SerializeField] TextMeshProUGUI title;
    private static string VideoUrl;
    public static string GetVideoUrl()
    {
        return VideoUrl;
    }
    LessonDTO lesson;
    // Start is called before the first frame update
    async void Start()
    {

        int lessonId = int.Parse(LessonList.GetLessonId());

        var lessonResponse = await lessonBUS.GetVideoByLessonId(lessonId);

        if (lessonResponse.data != null)
        {
            lesson=lessonResponse.data;
            VideoUrl = lesson.video_url;
            title.text = lesson.name;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickDoingExercise()
    {
        SceneHistory.GetInstance().LoadScene(GlobalVariable.EXERCISES_SCENE);
    }
    public void OnClickViewLesson()
    {
        //if video url
        SceneHistory.GetInstance().LoadScene("VideoLearning");
    }
}

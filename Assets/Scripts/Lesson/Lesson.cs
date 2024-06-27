using EasyUI.Progress;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lesson : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] GameObject playGameButton;
    [SerializeField] GameObject playGameIcon;
    [SerializeField] TextMeshProUGUI gameName;
    [SerializeField] GameObject learningButton;
    [SerializeField] GameObject learningIcon;
    [SerializeField] GameObject doingExercisesButton;
    [SerializeField] GameObject doingExercisesIcon;

    LessonBUS lessonBUS = new LessonBUS();
    GameBUS gameBUS = new GameBUS();

    private static string VideoUrl;
    public static string GetVideoUrl()
    {
        return VideoUrl;
    }
    
    LessonDTO lesson;
    GameDTO game = null;
    
    // Start is called before the first frame update
    async void Start()
    {
        playGameButton.SetActive(false);
        playGameIcon.SetActive(false);
        learningButton.SetActive(false);
        learningIcon.SetActive(false);

        int lessonId = int.Parse(LessonList.GetLessonId());
        var lessonResponse = await lessonBUS.GetVideoByLessonId(lessonId);
        if (lessonResponse.data != null)
        {
            learningButton.SetActive(true);
            learningIcon.SetActive(true);
            lesson = lessonResponse.data;
            VideoUrl = lesson.video_url;
            title.text = lesson.name;

        }
        Progress.Show("?ang t?i...", ProgressColor.Orange);
        var gameResponse = await gameBUS.GetGameDataByLessonId(lessonId);
        Progress.Hide();

        if (gameResponse.isSuccessful)
        {
            Debug.Log(gameResponse.data.gameConfig.game_type_name);
            game = gameResponse.data;
            playGameButton.SetActive(true);
            playGameIcon.SetActive(true);
            Debug.Log("CC" + game.gameConfig.game_type_name_vi);
            gameName.text = game.gameConfig.game_type_name_vi;
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
        if (lesson.book_url != null)
        {
            SceneHistory.GetInstance().LoadScene("BookLearningScene");
        }
        else
        {
            SceneHistory.GetInstance().LoadScene("VideoLearning");
        }
    }
    public void OnClickPlayGame()
    {
        if (game != null)
        {
            string sceneGameName = game.gameConfig.game_type_name + "Game";
                Debug.Log(sceneGameName);
            SceneHistory.GetInstance().LoadScene(sceneGameName);
        }
    }
}

using EasyUI.Progress;
using System;
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

    [SerializeField] GameObject NewNoteNotification;
    private float speed = 800f;
    private float topY = 1800f;
    private float bottomY = 1000f;
    private Vector3 direction = Vector3.down;
    private AudioSource audioSource;
    [SerializeField] AudioClip winSound;

    LessonBUS lessonBUS = new LessonBUS();
    GameBUS gameBUS = new GameBUS();
    NoteBUS noteBUS = new NoteBUS();

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
        NewNoteNotification.SetActive(false);
        playGameButton.SetActive(false);
        playGameIcon.SetActive(false);
        learningButton.SetActive(false);
        learningIcon.SetActive(false);

        int lessonId = int.Parse(LessonList.GetLessonId());
        LoadLessonData(lessonId);
        LoadGameData(lessonId);
        HandleAddNote(lessonId);
    }

    async void HandleAddNote(int lessonId)
    {
        string prevScene = SceneHistory.GetInstance().GetLastScene(); ;
        if (prevScene == GlobalVariable.VIDEO_LEARNING_SCENE || prevScene == GlobalVariable.BOOK_LEARNING_SCENE)
        {
            var checkExistRes = await noteBUS.CheckNoteExistsWithUserId(lessonId);
            if (checkExistRes.isSuccessful)
            {
                if (!checkExistRes.data.exists)
                {
                    var addNoteRes = await noteBUS.AddNewNoteWithUserId(lessonId);
                    if (addNoteRes.isSuccessful)
                    {
                        OpenNewNoteNotification();
                        audioSource = GetComponent<AudioSource>();
                        audioSource.clip = winSound;
                        audioSource.Play();
                    }
                }
            }
        }
        return;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private async void LoadLessonData(int lessonId)
    {
        var lessonResponse = await lessonBUS.GetVideoByLessonId(lessonId);
        if (lessonResponse.data != null)
        {
            learningButton.SetActive(true);
            learningIcon.SetActive(true);
            lesson = lessonResponse.data;
            VideoUrl = lesson.video_url;
            title.text = lesson.name;
        }


    }

    private async void LoadGameData(int lessonId)
    {
        var gameResponse = await gameBUS.GetGameDataByLessonId(lessonId);
        if (gameResponse.isSuccessful)
        {
            Debug.Log(gameResponse.data.gameConfig.game_type_name);
            game = gameResponse.data;
            playGameButton.SetActive(true);
            playGameIcon.SetActive(true);
            gameName.text = game.gameConfig.game_type_name_vi;
        }

    }

    public void OnClickDoingExercise()
    {
        SceneHistory.GetInstance().LoadScene(GlobalVariable.EXERCISES_SCENE);
    }

    public void OnClickViewLesson()
    {
        if (lesson.book_url != null)
        {
            SceneHistory.GetInstance().LoadScene(GlobalVariable.BOOK_LEARNING_SCENE);
        }
        else
        {
            SceneHistory.GetInstance().LoadScene(GlobalVariable.VIDEO_LEARNING_SCENE);
        }
    }

    public void OnClickPlayGame()
    {
        if (game != null)
        {
            string sceneGameName = game.gameConfig.game_type_name + "Game";
            SceneHistory.GetInstance().LoadScene(sceneGameName);
        }
    }

    private IEnumerator ShowNewNoteNotification()
    {
        NewNoteNotification.SetActive(true);
        while (true)
        {
            NewNoteNotification.transform.Translate(direction * speed * Time.deltaTime);
            if (NewNoteNotification.transform.position.y <= bottomY)
            {
                break;
            }
            yield return null;
        }
    }
    private IEnumerator HideNewNoteNotification()
    {
        while (true)
        {
            NewNoteNotification.transform.Translate(direction * speed * Time.deltaTime);
            if (NewNoteNotification.transform.position.y >= topY)
            {
                NewNoteNotification.SetActive(false);
                break;
            }
            yield return null;
        }
    }

    public void GoToNotesScene()
    {
        SceneHistory.GetInstance().LoadScene(GlobalVariable.NOTES_SCENE);
    }
    public void OpenNewNoteNotification()
    {
        StartCoroutine(ShowNewNoteNotification());
    }
    public void CloseNewNoteNotification()
    {
        StartCoroutine(HideNewNoteNotification());
    }
}

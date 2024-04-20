using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ExerciseMananger : MonoBehaviour
{
    // Start is called before the first frame update
    private List<ExerciseDTO> exercises;
    public int currentQuestion = 0;
    public int totalQuestion = 20;
    private int currentRightAnswer = 0;
    public TextMeshProUGUI progress;
    public Button CheckButton;
    TextMeshProUGUI textCheckBtn;
    Timer timer;

    //Drag drop exercise  object
    //Question List
    [SerializeField]
    Canvas DragDropExercise;
    [SerializeField]
    GameObject d_questionList;
    // Question Title
    [SerializeField]  
    TextMeshProUGUI d_title;
    //Answer List
    [SerializeField]
    GameObject d_answerList;
    //Result List
    [SerializeField]
    GameObject d_result;
    [SerializeField]
    GameObject d_answerSlot;
    public Sprite correctSprite;
    public Sprite incorrectSprite;

    [SerializeField]
    Canvas MultipleChoiceExercise;
    [SerializeField]
    GameObject m_answerList;
    [SerializeField]
    TMP_Text m_question;

    private void Awake()
    {
        textCheckBtn= CheckButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        timer= GetComponent<Timer>();
    }
    private async void Start()
    {
        var exerciseBUS=new ExerciseBUS();
        exercises= await exerciseBUS.GetAllExercises();
        if (exercises != null)
        {
            totalQuestion= exercises.Count;
            UpdateUI();
        }
        ));

    }

    // Update is called once per frame
    void Update()
    {
        if (currentQuestion + 1  >= totalQuestion)
        {
            textCheckBtn.text = "Nộp bài";
        }
    }
    IEnumerator  NextQuestion()
    {
        yield return new WaitForSeconds(1f);
        currentQuestion = currentQuestion + 1;
        if (currentQuestion >= totalQuestion)
        {

            Debug.Log($"Your result is: {currentRightAnswer}/{totalQuestion}");
            timer.isStop= true;
        }
        else
        {
           
            UpdateUI();
        }
        
        
    }
    void ChangeDragDropObject(ExerciseDTO exercise)
    {
        var questions = exercise.question.Split(",");
        var answers=exercise.answer.Split(",");
        var a = d_answerList.GetComponentsInChildren<TextMeshProUGUI>();
        var q = d_questionList.GetComponentsInChildren<TextMeshProUGUI>();
        for(int i=0;i<a.Length;i++)
        {
            a[i].text = answers[i];
            q[i].text = questions[i];
        }
       
    }
    void UpdateUI()
    {
        progress.text = string.Format("{0:00}/{1:00}", currentQuestion + 1, totalQuestion);
        if (exercises[currentQuestion].type == GlobalVariable.DragDropType && exercises != null)
        {
            DragDropExercise.gameObject.SetActive(true);
            ChangeDragDropObject(exercises[currentQuestion]);

        }
        else if (exercises[currentQuestion].type == GlobalVariable.MULTIPLE_CHOICE_TYPE && exercises != null)
        {
            MultipleChoiceExercise.enabled = true;
            ChangeMultipleChoiceObject(exercises[currentQuestion]);
        }
       
    }
    public void CheckDragDropAnswer()
    {
        bool isRight = true;
        var aslot = d_answerSlot.GetComponentsInChildren<TextMeshProUGUI>();
        
        var resultListImage = d_result.GetComponentsInChildren<Image>();
        if (aslot.Length != resultListImage.Length)
        {
            isRight = false;
            for (int i = 0; i < resultListImage.Length; i++)
            {
                
                    resultListImage[i].sprite = incorrectSprite; 
            }
        }
        else
        {
            var rightAnswer = exercises[currentQuestion].right_answer.Split(",");
            for (int i = 0; i < resultListImage.Length; i++)
            {
                if (aslot[i] && rightAnswer[i] == aslot[i].text)
                {
                    resultListImage[i].sprite = correctSprite;
                }
                else
                {
                    resultListImage[i].sprite = incorrectSprite;
                    isRight = false;

                }
            }
        }
       
        if(isRight )
        {
            currentRightAnswer += 1;
        }
        d_result.SetActive(true);
        StartCoroutine(NextQuestion());
    }


    void ChangeMultipleChoiceObject(ExerciseDTO exercise)
    {
        var questions = exercise.question;
        var answers = exercise.answer.Split(",");

        m_question.text = questions;

        Button[] buttons = m_answerList.GetComponentsInChildren<Button>();

        for (int i = 0; i < buttons.Length; i++)
        {
            TextMeshProUGUI tmp = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null)
            {
                tmp.text = answers[i];
            }
            else
            {
                Debug.LogError("TextMeshPro component not found in children of button.");
            }
        }

    }

    public void CheckMultipleChoiceAnswer()
    {
        var rightAnswer = exercises[currentQuestion].right_answer;

        Image[] answerObjects = m_answerList.GetComponentsInChildren<Image>();

        for (int i = 0; i < answerObjects.Length; i++)
        {
            TextMeshProUGUI tmp = answerObjects[i].GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null)
            {
                if (tmp.text == rightAnswer)
                {
                    answerObjects[i].color = HexToColor("#00FF1E");
                }
                else
                {
                    answerObjects[i].color = HexToColor("#FF0000");
                }
            }
            else
            {
                Debug.LogError("TextMeshPro component not found in children of button.");
            }
        }
    }
    private Color HexToColor(string hex)
    {
        Color color = Color.white;
        if (UnityEngine.ColorUtility.TryParseHtmlString(hex, out color))
        {
            return color;
        }
        else
        {
            Debug.LogError("Invalid hexadecimal color: " + hex);
            return Color.white;
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public partial class ExerciseMananger : MonoBehaviour
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
    Image Notification;
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


    [SerializeField] private Canvas InputExercise;

    [SerializeField] private TMP_Text i_question;

    [SerializeField] private TMP_InputField i_answer;

    [SerializeField] private GameObject i_holder;
    [SerializeField] private GameObject correct_answer;
    [SerializeField] private GameObject incorrect_answer;

    


    private bool blockIO = false;
    private bool hasHandledTimeout = false;

    private void Awake()
    {
        textCheckBtn= CheckButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        timer= GetComponent<Timer>();
        NextBtn.gameObject.SetActive(false);
        PrevBtn.gameObject.SetActive(false);
        ReviewResult.gameObject.SetActive(false);
        NextBtn.onClick.AddListener(OnClickNextButton);
        PrevBtn.onClick.AddListener(OnClickPrevButton);
    }
    
    private async void Start()
    {
        var exerciseBUS = new ExerciseBUS();
        string lessonId = LessonList.GetLessonId();
        try
        {
            exercises = await exerciseBUS.GetExercisesByLessonId(lessonId);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
   
        if (exercises != null)
        {
            totalQuestion= exercises.Count;
            UpdateUI();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!blockIO)
        {
            if (currentQuestion + 1 >= totalQuestion)
            {
                textCheckBtn.text = "Nộp bài";
            }
            if (ReviewList.gameObject.activeSelf || ReviewResult.gameObject.activeSelf)
            {
                textCheckBtn.text = "Thoát";
            }
            if (timer.timeValue == 0 && !hasHandledTimeout)
            {
                StartCoroutine(HandleTimeOut());
                hasHandledTimeout = true;
            }
        }
    }
    
    private void ShowResult()
    {
        MultipleChoiceExercise.gameObject.SetActive(false);
        InputExercise.gameObject.SetActive(false);
        DragDropExercise.gameObject.SetActive(false);
        ReviewResult.gameObject.SetActive(true);
        result.text = $"{currentRightAnswer}/{totalQuestion}";
        timer.isStop = true;
    }

    IEnumerator  NextQuestion()
    {
        blockIO = true;
        yield return new WaitForSeconds(1f);
        blockIO = false;
        currentQuestion = currentQuestion + 1;
        if (currentQuestion >= totalQuestion)
        {       
            ShowResult();
            yield return null;
        }
        
        if (currentQuestion < totalQuestion)
        {
            UpdateUI();
        }    
    }

    IEnumerator HandleTimeOut()
    {
        while(currentQuestion < totalQuestion)
        {
            UpdateUI();
            AddExerciseToReviewList();
            currentQuestion += 1;
            

        }
        ShowResult();
        yield return null;
    }

    void ChangeDragDropObject(ExerciseDTO exercise)
    {
        d_result.SetActive(false);
        var questions = exercise.question.Split(",");
        var answers=exercise.answer.Split(",");
       
        var aslotItem = d_answerSlot.GetComponentsInChildren<DragAndDrop>();
        if (aslotItem.Length > 0)
        {
            var alist = d_answerList.transform.GetComponentsInChildren<Transform>();
            int j = 0;
            for (int i = 0; i < alist.Length; i++)
            {
                if (alist[i].name.Contains("ItemContain"))
                {
                    aslotItem[j].transform.SetParent(alist[i]);
                    Debug.Log(alist[i].name);
                    j++;
                }

            }
        }
        var a = d_answerList.GetComponentsInChildren<TextMeshProUGUI>();

        var q = d_questionList.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i=0;i<a.Length;i++)
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
            MultipleChoiceExercise.gameObject.SetActive(false);
            InputExercise.gameObject.SetActive(false);
            DragDropExercise.gameObject.SetActive(true);
            ChangeDragDropObject(exercises[currentQuestion]);


        }
        else if (exercises[currentQuestion].type == GlobalVariable.MULTIPLE_CHOICE_TYPE && exercises != null)
        {
            ResetMultipleChoiceAnswerAttribute();
            DragDropExercise.gameObject.SetActive(false);
            InputExercise.gameObject.SetActive(false);
            MultipleChoiceExercise.gameObject.SetActive(true);
            ChangeMultipleChoiceObject(exercises[currentQuestion]);
        }
        else if (exercises[currentQuestion].type == GlobalVariable.INPUT_TYPE && exercises != null){
            InputExercise.gameObject.SetActive(true);
            MultipleChoiceExercise.gameObject.SetActive(false);
            DragDropExercise.gameObject.SetActive(false);
             correct_answer.SetActive(false);
            incorrect_answer.SetActive(false);
            i_answer.text="";
            ChangeInputObject(exercises[currentQuestion]);
        }
       
    }

    public void ChangeInputObject(ExerciseDTO exercise){
        var question = exercise.question;
        var right_answers = exercise.right_answer.Split(",");
        Debug.Log(right_answers.Length);

        i_question.text = question;
    
        if (right_answers.Length > 1) {

         for (int i = 1; i < right_answers.Length; i++){

            
             TMP_InputField newInput = Instantiate(i_answer);
            newInput.transform.SetParent(i_answer.transform.parent.transform, false);
            newInput.transform.SetSiblingIndex(i);
            
         }
        }
        else {
           TMP_InputField[] inputList = i_holder.GetComponentsInChildren<TMP_InputField>();

            for (int i = 1; i < inputList.Length; i++){
                Destroy(inputList[i].gameObject);
            }

        }
    }

    public void CheckInputAnswer(){
        var right_answers = exercises[currentQuestion].right_answer.Split(",");
        TMP_InputField[] inputList = i_holder.GetComponentsInChildren<TMP_InputField>();
            bool check = true;
            for (int i = 0; i < inputList.Length; i++){
               if (inputList[i].text != right_answers[i]){
                    check = !check;
                    break;
               }
            }

            if (check){
                currentRightAnswer+=1;
                correct_answer.SetActive(true);
            }
            else {
                incorrect_answer.SetActive(true);
            }

        
            StartCoroutine(NextQuestion());
    }
    
    public void hideNotification()
    {
        Notification.gameObject.SetActive(false);
    }
    
    public void CheckDragDropAnswer()
    {
        bool isRight = true;
        var aslot = d_answerSlot.GetComponentsInChildren<TextMeshProUGUI>();
        
        var resultListImage = d_result.GetComponentsInChildren<Image>();
        if (aslot.Length != resultListImage.Length)
        {
            Notification.gameObject.SetActive(true);
            return;
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
        AddExerciseToReviewList();
        StartCoroutine(NextQuestion());
        d_result.SetActive(true);
        
        
    }

    public void CheckAnswers()
    {
        if (!blockIO)
        {
            if (currentQuestion >= totalQuestion)
            {
                SceneHistory.GetInstance().LoadScene(GlobalVariable.MAIN_SCENE);
            }
            if (exercises[currentQuestion].type == GlobalVariable.DragDropType && exercises != null)
            {
                CheckDragDropAnswer();               
            }
            else if (exercises[currentQuestion].type == GlobalVariable.MULTIPLE_CHOICE_TYPE && exercises != null)
            {
                CheckMultipleChoiceAnswer();
            }
            else if (exercises[currentQuestion].type == GlobalVariable.INPUT_TYPE && exercises != null)
            {

                CheckInputAnswer();
                AddExerciseToReviewList();
            }
        }
            
    }
}

using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ExamManager : MonoBehaviour
{
    ExerciseBUS exerciseBUS;
    QuestionResultBUS questionResultBus; 
    TestResultBUS testResultBus; 
    // Start is called before the first frame update
    public List<ExerciseDTO> exercises;
    public int currentQuestion = 0;
    public int totalQuestion = 20;
    private int currentRightAnswer = 0;
    public TextMeshProUGUI progress;
    public Button CheckButton;
    TextMeshProUGUI textCheckBtn;
    Timer timer;
    public List<QuestionResultDTO> questionResultList;
    TestResultDTO testResult;

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

    [SerializeField]
    Canvas MultipleChoiceExercise;
    [SerializeField]
    GameObject m_answerList;
    [SerializeField]
    TMP_Text m_question;
    private Button[] buttonList;

    [SerializeField] private Canvas InputExercise;

    [SerializeField] private TMP_Text i_question;

    [SerializeField] private TMP_InputField i_answer;

    [SerializeField] private GameObject i_holder;
     [SerializeField] private GameObject correct_answer;
      [SerializeField] private GameObject incorrect_answer;


    private void Awake()
    {
        textCheckBtn= CheckButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        timer= GetComponent<Timer>();
        buttonList=m_answerList.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttonList.Length; i++)
        {
            // Add a listener to the button's onClick event
            int index = i; // Capture the current value of i for use in the listener
            buttonList[i].onClick.AddListener(() => OnSelectedAnswer(index));
        }
    }
    private async void Start()
    {
        exerciseBUS = new ExerciseBUS();
        questionResultBus = new QuestionResultBUS();
        testResultBus = new TestResultBUS();

        testResult = new TestResultDTO()
        {
            test_id = 1,
            user_id = 38,
            //user_id = PlayerPrefs.GetInt(GlobalVariable.userID),
        };
        var testResultResponse= await testResultBus.GetById(1);
        if(testResultResponse.data != null)
        {
            testResult = testResultResponse.data;
            Debug.Log("testResultResponse " + testResultResponse.data.test_id.ToString());
        }
        
        var response = await exerciseBUS.GetExerciseByTestId(1);
        //var response = await exerciseBUS.GetExerciseByType(new ExerciseTypeDTO
        //{
        //    type = GlobalVariable.MULTIPLE_CHOICE_TYPE
        //});

        if (response.data != null)
        {
            exercises = response.data;

            totalQuestion = exercises.Count;
            questionResultList = new List<QuestionResultDTO>();
            UpdateUI();

        }
        else
        {
            Debug.Log($"Error: {response.message}");

        }
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
        TMP_InputField[] inputList = i_holder.GetComponentsInChildren<TMP_InputField>();
        var user_answer = inputList[0].text;

        for (int i = 1; i < inputList.Length; i++){
            user_answer = user_answer + "," + inputList[i].text; 
        }

        var temp = new QuestionResultDTO()
        {
            exercise_id = exercises[currentQuestion].exercise_id,
            test_result_id = testResult.test_result_id,
            user_answer = user_answer,
        };

        questionResultList.Add(temp);
        StartCoroutine(NextQuestion());
    }
    public void hideNotification()
    {
        Notification.gameObject.SetActive(false);
    }
    void ChangeDragDropObject(ExerciseDTO exercise)
    {
        d_result.SetActive(false);
        var questions = exercise.question.Split(",");
        var answers = exercise.answer.Split(",");

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
        for (int i = 0; i < a.Length; i++)
        {
            a[i].text = answers[i];
            q[i].text = questions[i];
        }

    }
    public void CheckDragDropAnswer()
    {
       
        var aslot = d_answerSlot.GetComponentsInChildren<TextMeshProUGUI>();
        
        var resultListImage = d_result.GetComponentsInChildren<Image>();
        if (aslot.Length != resultListImage.Length)
        {
            Notification.gameObject.SetActive(true);
            return;
        }
        else
        {
            string user_answer = string.Join(",", aslot[0].text, aslot[1].text, aslot[2].text);
           
            var temp = new QuestionResultDTO()
            {
                exercise_id = exercises[currentQuestion].exercise_id,
                test_result_id=testResult.test_result_id,
                user_answer= user_answer,
            };
            questionResultList.Add(temp);
        }
       
        
        StartCoroutine(NextQuestion());   
    }

    private void OnSelectedAnswer(int buttonIndex)
    {
        Debug.Log("Button " + buttonIndex + " clicked!");

        // Change the color of the clicked button's image component

        Image buttonImage = buttonList[buttonIndex].GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = Color.blue;
        }
        else
        {
            Debug.LogWarning("Button " + buttonIndex + " doesn't have an Image component!");
        }
        for (int i = 0; i < buttonList.Length; i++)
        {
            if (i != buttonIndex)
            {
                Image buttonImages = buttonList[i].GetComponent<Image>();
                buttonImages.color = Color.white;
            }

        }
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
            var image = buttons[i].GetComponent<Image>();
            image.color = Color.white;
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
            Image img = answerObjects[i].GetComponent<Image>();
            if (img != null && img.color == Color.blue)
            {
                var user_answer = tmp.text;
                var temp = new QuestionResultDTO()
                {
                    exercise_id = exercises[currentQuestion].exercise_id,
                    test_result_id = testResult.test_result_id,
                    user_answer = user_answer,
                };
                questionResultList.Add(temp);
            }
            else
            {
                Debug.LogError("TextMeshPro component not found in children of button.");
            }
        }


        StartCoroutine(NextQuestion());
    }

    public void CheckAnswers()
    {
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
            StartCoroutine(NextQuestion());
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

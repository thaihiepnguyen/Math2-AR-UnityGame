using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using EasyUI;
using EasyUI.Toast;

public class ExamManager : MonoBehaviour
{
    ExerciseBUS exerciseBUS;
    QuestionResultBUS questionResultBus; 
    TestResultBUS testResultBus; 
    TestBUS testBus;
    // Start is called before the first frame update
    public List<ExerciseDTO> exercises;
    public int currentQuestion = 0;
    public int totalQuestion = 20;
    public int currentRightAnswer = 0;
    public TextMeshProUGUI progress;
    public TextMeshProUGUI time;
    public Button CheckButton;
    TextMeshProUGUI textCheckBtn;
    [HideInInspector]public Timer timer;
    public List<QuestionResultDTO> questionResultList;
    TestResultDTO testResult;
    [SerializeField] Canvas reviewUI;
    [SerializeField] TextMeshProUGUI title;
    
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
    [SerializeField]
    Image imageQuestion;
    private Button[] buttonList;
    private bool isImageQuestion = false;

    [SerializeField] private Canvas InputExercise;

    [SerializeField] private TMP_Text i_question;

    [SerializeField] private TMP_InputField i_answer;

    [SerializeField] private GameObject i_holder;
     [SerializeField] private GameObject correct_answer;
      [SerializeField] private GameObject incorrect_answer;

    private int test_id;
    private void Awake()
    {
        test_id=ExamListManager.GetTestID();
        questionResultList = new List<QuestionResultDTO>();
        textCheckBtn = CheckButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
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
        testBus = new TestBUS();
        testResult = new TestResultDTO()
        {
            test_id = test_id,
            //user_id = 38,
            user_id = PlayerPrefs.GetInt(GlobalVariable.userID),
        };
        //var testResultResponse= await testResultBus.GetById(1);
          var testResultResponse= await testResultBus.AddTestResult(testResult);
        var testResponse = await testBus.GetById(test_id);
        if(testResultResponse.data != null)
        {
            testResult = testResultResponse.data;
            Debug.Log("testResultResponse " + testResultResponse.data.test_id.ToString());
            
        }
        if(testResponse.data != null)
        {
            title.text = $"Bài thi học kỳ {Semester.GetSemester()} - {testResponse.data.test_name}";
            Debug.Log("testResponse " + testResponse.data.test_id.ToString());
        }
        var response = await exerciseBUS.GetExerciseByTestId(test_id);
        //var response = await exerciseBUS.GetExerciseByType(new ExerciseTypeDTO
        //{
        //    type = GlobalVariable.MULTIPLE_CHOICE_TYPE
        //});

        if (response.data != null)
        {
            exercises = response.data;

            totalQuestion = exercises.Count;
            
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
        if(timer.timeValue == 0f)
        {
            if (currentQuestion + 1 <= totalQuestion)
            {
               
                if (questionResultList.Count <= totalQuestion)
                {
                    for(int i=currentQuestion; i<totalQuestion; i++)
                    {
                        var temp = new QuestionResultDTO()
                        {
                            exercise_id = exercises[i].exercise_id,
                            test_result_id = testResult.test_result_id,
                            user_answer = "",
                        };
                        questionResultList.Add(temp);
                    }
                    currentQuestion= totalQuestion;
                    NextQuestion();
                    ShowReviewUI();
                }
                
                return;
            }
            
            
        }
    }
    void ShowReviewUI()
    {
        var border = GameObject.Find("Border");
        if (border != null)
        {
            border.SetActive(false);
        }
        
        DragDropExercise.gameObject.SetActive(false);
        InputExercise.gameObject.SetActive(false);
        MultipleChoiceExercise.gameObject.SetActive(false);
        CheckButton.gameObject.SetActive(false);
        CheckButton.gameObject.SetActive(false);
        time.gameObject.SetActive(false);
        progress.gameObject.SetActive(false);
        reviewUI.gameObject.SetActive(true);


    }
    async void  NextQuestion()
    {
        currentQuestion = currentQuestion + 1;
        if (currentQuestion >= totalQuestion)
        {
            for (int i = 0;i< questionResultList.Count; i++)
            {
                var response = await questionResultBus.AddQuestionResult(questionResultList[i]);
                if (questionResultList[i].user_answer == exercises[i].right_answer)
                {
                    currentRightAnswer += 1;
                }
            }
            Debug.Log($"Your result is: {currentRightAnswer}/{totalQuestion}");
            timer.isStop= true;
            ShowReviewUI();
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
        if(inputList.Length>1)
        {
            if (inputList[0].text == "" || inputList[1].text == "")
            {
                
                Toast.Show("Bạn hãy hoàn thành câu hỏi của mình nhé", .7f, ToastPosition.MiddleCenter);
                return;
            }
            
        }
        else
        {
            if (inputList[0].text == "")
            {
                Toast.Show("Bạn hãy hoàn thành câu hỏi của mình nhé", .7f, ToastPosition.MiddleCenter);
                return;
            }
        }
        var temp = new QuestionResultDTO()
        {
            exercise_id = exercises[currentQuestion].exercise_id,
            test_result_id = testResult.test_result_id,
            user_answer = user_answer,
        };

        questionResultList.Add(temp);
        NextQuestion();
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
            Toast.Show("Bạn hãy hoàn thành câu hỏi của mình nhé", .7f, ToastPosition.MiddleCenter);
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
       
        
        NextQuestion();   
    }

    private void OnSelectedAnswer(int buttonIndex)
    {
        Debug.Log("Button " + buttonIndex + " clicked!");

        // Change the color of the clicked button's image component

        Image buttonImage = buttonList[buttonIndex].GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = HexToColor("#FFB45D");
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
        if (exercise.image_id != null)
        {
            isImageQuestion = true;
            StartCoroutine(LoadImageManager.LoadBinaryImage(imageQuestion, (int)exercise.image_id));
            Vector3 currentPosition = m_answerList.transform.position;
            currentPosition.x += 460f;
            m_answerList.transform.position = currentPosition;
            imageQuestion.gameObject.SetActive(true);
        }

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
        bool isAnswer = false;
        for (int i = 0; i < answerObjects.Length; i++)
        {
            TextMeshProUGUI tmp = answerObjects[i].GetComponentInChildren<TextMeshProUGUI>();
            Image img = answerObjects[i].GetComponent<Image>();
            if (img != null && img.color == HexToColor("#FFB45D"))
            {
                var user_answer = tmp.text;
                var temp = new QuestionResultDTO()
                {
                    exercise_id = exercises[currentQuestion].exercise_id,
                    test_result_id = testResult.test_result_id,
                    user_answer = user_answer,
                };
                questionResultList.Add(temp);
                isAnswer = true;
            }  

        }
        if (isImageQuestion == true)
        {
            isImageQuestion = false;
            imageQuestion.gameObject.SetActive(false);
            Vector3 currentPosition = m_answerList.transform.position;
            currentPosition.x -= 460f;
            m_answerList.transform.position = currentPosition;
        }
        if (!isAnswer)
        {
            Toast.Show("Bạn hãy hoàn thành câu hỏi của mình nhé", .7f, ToastPosition.MiddleCenter);
            return;
        }


        NextQuestion();
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

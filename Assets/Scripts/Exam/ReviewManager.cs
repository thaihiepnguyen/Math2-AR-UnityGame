using EasyUI.Progress;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public partial class ReviewManager : MonoBehaviour
{
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Canvas dragDropResult;
    [SerializeField] private Canvas multipleChoiceResult;
    [SerializeField] private Canvas inputResult;
    [SerializeField] private Canvas ResultUI;
    [SerializeField] private GameObject ExerciseUI;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] public TextMeshProUGUI progress;
    [SerializeField] Button backToResultBtn;
    [SerializeField] private TextMeshProUGUI title;
    ExamManager examManager;
    List<ExerciseDTO> exercises;
    List<QuestionResultDTO> questionResults;
    int currentQuestion = 0;
    TestResultBUS testResultBUS;
    ExerciseBUS exerciseBUS;
    QuestionResultBUS questionResultBUS;
    TestResultDTO testResult;

    async void Start()
    {
        if (DetailExamManager.GetisReview())
        {
            exerciseBUS = new ExerciseBUS();
            testResultBUS = new TestResultBUS();
            questionResultBUS = new QuestionResultBUS();
            Progress.Show("Đang tải",ProgressColor.Orange);
            var exerciseResponse = await exerciseBUS.GetExerciseByTestId(ExamListManager.GetTestID());

            var testResultResponse = await testResultBUS.GetByUserIdAndTestId(ExamListManager.GetTestID());
            Progress.Hide();
            if (exerciseResponse.data != null)
            {
                exercises = exerciseResponse.data;
            }
            if (testResultResponse.data != null)
            {
                testResult = testResultResponse.data;

            }
            var questionResultResponse = await questionResultBUS.GetQuestionResultByTestResultId(testResultResponse.data.test_result_id);
            if (questionResultResponse.data != null)
            {
                questionResults = questionResultResponse.data;
                title.text = DetailExamManager.GetTitle();
                resultText.text = $"Bạn đã trả lời đúng {testResult.point} câu trong {testResult.completed_time}";
                updateUI();
            }

        }
        else
        {
            examManager = mainCanvas.GetComponent<ExamManager>();
            if (examManager != null)
            {

                questionResults = examManager.questionResultList;
                exercises = examManager.exercises;
                resultText.text = $"Bạn đã trả lời đúng {examManager.currentRightAnswer}/{examManager.totalQuestion} câu trong {examManager.timer.toTimeString()}";
                updateUI();
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (questionResults != null)
        {
            if (currentQuestion + 1 >= questionResults.Count)
            {
                progress.gameObject.SetActive(false);
                backToResultBtn.gameObject.SetActive(true);
            }
            else
            {
                progress.gameObject.SetActive(true);
                backToResultBtn.gameObject.SetActive(false);
            }
        }

    }



    void updateUI()
    {
        progress.text = string.Format("{0:00}/{1:00}", currentQuestion + 1, questionResults.Count);
        if (exercises[currentQuestion].type == GlobalVariable.DragDropType && exercises != null)
        {
            dragDropResult.gameObject.SetActive(true);
            inputResult.gameObject.SetActive(false);
            multipleChoiceResult.gameObject.SetActive(false);

            ChangeDragDropObject(exercises[currentQuestion], questionResults[currentQuestion]);


        }
        else if (exercises[currentQuestion].type == GlobalVariable.MULTIPLE_CHOICE_TYPE && exercises != null)
        {
            dragDropResult.gameObject.SetActive(false);
            inputResult.gameObject.SetActive(false);
            multipleChoiceResult.gameObject.SetActive(true);
            ChangeMultipleChoiceObject(exercises[currentQuestion], questionResults[currentQuestion]);
        }
        else if (exercises[currentQuestion].type == GlobalVariable.INPUT_TYPE && exercises != null)
        {

            dragDropResult.gameObject.SetActive(false);
            inputResult.gameObject.SetActive(true);
            multipleChoiceResult.gameObject.SetActive(false);
            ChangeInputObject(exercises[currentQuestion], questionResults[currentQuestion]);
        }
    }
    public void onRedo()
    {
        SceneManager.LoadScene(GlobalVariable.EXAM_SCENE);
    }
    public void onViewReview()
    {
        ResultUI.gameObject.SetActive(false);
        ExerciseUI.gameObject.SetActive(true);
        updateUI();
    }
    public void onExit()
    {
        SceneHistory.GetInstance().PreviousScene();
    }
    public void NextQuestion()
    {
        currentQuestion++;

        if (currentQuestion >= questionResults.Count)
        {
            currentQuestion = questionResults.Count - 1;
            return;
        }
        updateUI();
    }
    public void PreviousQuestion()
    {
        currentQuestion--;

        if (currentQuestion < 0)
        {
            currentQuestion = 0;
            return;
        }
        updateUI();
    }
    public void BackToResultUI()
    {
        ResultUI.gameObject.SetActive(true);
        ExerciseUI.gameObject.SetActive(false);
    }
}

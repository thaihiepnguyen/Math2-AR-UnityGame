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
    ExamManager examManager;
    List<ExerciseDTO> exercises;
    List<QuestionResultDTO> questionResults;
    int currentQuestion = 0;
    //DragDrop Result Object

    //Multiple Choices Result Object

    //Input Result Object

    //[SerializeField] private Button RedoBtn;
    //[SerializeField] private Button ReviewBtn;
    //[SerializeField] private Button ExitBtn;
    // Start is called before the first frame update

     void Start()
    {
        examManager = mainCanvas.GetComponent<ExamManager>();
        if(examManager!= null )
        {
           
            questionResults = examManager.questionResultList;
            exercises=examManager.exercises;
            resultText.text = $"Bạn đã trả lời đúng {examManager.currentRightAnswer}/{examManager.totalQuestion} câu trong {examManager.timer.toTimeString()} phút!!";
            updateUI();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(currentQuestion + 1 >=questionResults.Count)
        {
            progress.gameObject.SetActive(false);
            backToResultBtn.gameObject.SetActive(true);
        }
        else
        {
            progress.gameObject.SetActive(true  );
            backToResultBtn.gameObject.SetActive(false);
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
        //SceneManager.LoadScene(GlobalVariable.EXAM_LIST)
    }
    public void NextQuestion()
    {
        currentQuestion++;
        
        if (currentQuestion >= examManager.totalQuestion)
        {
            return;
        }
        updateUI();
    }
    public void PreviousQuestion()
    {
        currentQuestion--;
        
        if (currentQuestion < 0)
        {
            currentQuestion= 0;
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

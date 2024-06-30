using EasyUI.Progress;
using EasyUI.Toast;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DetailExamManager : MonoBehaviour
{
    // Start is called before the first frame update
    TestBUS testBus;
    TestDTO test;
    TestResultBUS testresultBus = new TestResultBUS();
    TestResultDTO testResultDTO;
    [SerializeField]  public TextMeshProUGUI title;
    static public string titleText;
    private bool canReview = true;
    public static string GetTitle()
    {
        return titleText;
    }
    private static bool isReview = false;
    public static bool GetisReview()
    {
        return isReview;
    }
    async void Start()
    {
        
        testBus = new TestBUS();
        Progress.Show("Đang tải",ProgressColor.Orange);
        var response= await testBus.GetById(ExamListManager.GetTestID());
        
        var testResultResponse = await testresultBus.GetByUserIdAndTestId(ExamListManager.GetTestID());
        Progress.Hide();
        if (response.data != null)
        {
            test=response.data;
            title.text = $"Đề thi HK {Semester.GetSemester()} - {test.test_name}";
            titleText=title.text;
        }
        if (testResultResponse.data != null)
        {
            testResultDTO= testResultResponse.data;
            Debug.Log(testResultDTO.date);
            if (testResultDTO.completed_time == null)
            {
                canReview = false;
            }

        }
       
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public  void onStartExam()
    {
        isReview = false;
        
        SceneHistory.GetInstance().LoadScene("ExamScene");
    }
    public void onReviewExam()
    {
        if(canReview)
        {
            isReview = true;
            SceneHistory.GetInstance().LoadScene("ReviewExamScene");
        }
        else
        {
            Toast.Show("Bé chưa hoàn thành bài thi này!", 1f, ToastPosition.MiddleCenter);
        }
    }
}

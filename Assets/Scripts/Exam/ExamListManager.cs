using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExamListManager : MonoBehaviour
{
    // Start is called before the first frame update
    private TestPurchaseBUS testPurchaseBUS= new TestPurchaseBUS();
    private TestBUS testBUS= new TestBUS();
    List<TestDTO> testList = new List<TestDTO>();
    List<TestPurchaseDTO> testPurchaseList = new List<TestPurchaseDTO>();
    [SerializeField] GameObject content;
    [SerializeField] GameObject exam;
    [SerializeField] TextMeshProUGUI title;
    private int semester;
    private static int test_id;
    private Button[] examList;
    public static int GetTestID()
    {
        return test_id;
    }
    async void Start()
    {
        semester=Semester.GetSemester();
        title.text = $"Đề Kiểm Tra Học Kỳ {semester}";
        var test_response = await testBUS.GetTestsBySemester(semester);
        //var test_response = await testBUS.GetTestsBySemester(1);
        int uid = PlayerPrefs.GetInt(GlobalVariable.userID);
        Debug.Log(uid);
        //var testPurchase_response= await testPurchaseBUS.GetByUserId(36);
        var testPurchase_response= await testPurchaseBUS.GetByUserId();
        if (test_response.data != null)
        {
            testList= test_response.data;
        }
        if(testPurchase_response.data != null)
        {
            testPurchaseList = testPurchase_response.data;
            
        }
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void UpdateUI()
    {
        if ( testList.Count > 0)
        {
            for (int i = 1; i < testList.Count; i++)
            {
                var newExam = Instantiate(exam);
                newExam.transform.SetParent(content.transform);
            }


            examList = content.GetComponentsInChildren<Button>();
            for (int i = 0; i < testList.Count; i++)
            {
                var btnText = examList[i].GetComponentsInChildren<TextMeshProUGUI>();
                int index = i;
                examList[i].onClick.AddListener(() => onExamBtnClicked(index));
                if (testList[i].status == "Free")
                {
                    btnText[0].text = "Miễn phí";
                }
                else
                {
                    if (isPaidExam(testList[i], testPurchaseList))
                    {
                        btnText[0].text = "Đã mua";
                        btnText[0].color = Color.green;
                    }
                    else
                    {
                        btnText[0].text = "Bị khóa";
                        btnText[0].color = Color.red;
                        examList[i].interactable = false;
                    }
                }
                btnText[1].text = "Đề thi HK" + testList[i].semester + " " + testList[i].test_name;
                
            }
        }
    }
    private bool isPaidExam(TestDTO test,List<TestPurchaseDTO> testPurchases)
    {
        for(int i = 0; testPurchases.Count > i; i++)
        {
            if (test.test_id == testPurchases[i].test_id)
            {
                return true;
            }
        }
        return false;
    }
    public void onExamBtnClicked(int btnIndex)
    {
        Debug.Log("Btn " + btnIndex + " clicked");
        test_id = testList[btnIndex].test_id;
        SceneHistory.GetInstance().LoadScene("DetailExamScene");
    }
}

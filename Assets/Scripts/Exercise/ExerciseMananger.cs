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
    private void Awake()
    {
        
    }
    private async void Start()
    {
        StartCoroutine(GetRequest(GlobalVariable.server_url + "/exercises", (response)=> { 
        
            UpdateUI();
        }
        ));

    }
    //Send request
    public delegate void ExerciseResponseCallback(ExerciseResponseDTO response);
    IEnumerator GetRequest(string uri, ExerciseResponseCallback callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string json = webRequest.downloadHandler.text;
                var exerciseResponse = JsonConvert.DeserializeObject<ExerciseResponseDTO>(json);
                exercises = exerciseResponse.data;
                callback?.Invoke(exerciseResponse);
            }
            else
            {
                Debug.LogError(webRequest.error);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NextQuestion()
    {
        currentQuestion = currentQuestion + 1;
        UpdateUI();
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
        if (exercises[currentQuestion].type == GlobalVariable.DragDropType && exercises != null)
        {
            DragDropExercise.enabled = true;
            ChangeDragDropObject(exercises[currentQuestion]);

        }
       
    }
    public void CheckDragDropAnswer()
    {
        var aslot = d_answerSlot.GetComponentsInChildren<TextMeshProUGUI>();
        
        var resultListImage = d_result.GetComponentsInChildren<Image>();
        if (aslot.Length != resultListImage.Length)
        {
            
            return;
        }
        var rightAnswer= exercises[currentQuestion].right_answer.Split(",");
        for (int i=0;i< resultListImage.Length;i++)
        {
            if ( aslot[i] && rightAnswer[i] == aslot[i].text )
            {
                resultListImage[i].sprite=correctSprite;
            }
            else
            {
                resultListImage[i].sprite = incorrectSprite;
            }
        }
        d_result.SetActive(true);
    }
}

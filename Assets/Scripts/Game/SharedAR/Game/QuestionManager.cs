using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class QuestionManager : NetworkBehaviour
{
    public static QuestionManager Instance;
    [SerializeField] TextMeshProUGUI progress;
    [SerializeField] TextMeshProUGUI question;
    [SerializeField] GameObject collectibles;
    private List<GameData> gameDataList;
    private GameDTO gameDTO;
    private int currentQuestion=0;
    private int totalQuestion;
    private GameBUS gameBUS = new GameBUS();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;
    }
    // Start is called before the first frame update
    async void Start()
    {
        var gameResponse= await gameBUS.GetGameDataByLessonId(1);
        if(gameResponse.isSuccessful)
        {
            gameDTO = gameResponse.data;
            gameDataList = gameDTO.gameData.ToList();
            totalQuestion = gameDataList.Count();
            UpdateUI();
            collectibles.SetActive(true);
        }
        //StartGameAR.OnStartSharedSpace += OnHostStart;
    }

    private void OnHostStart()
    {
        collectibles.SetActive(true);
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void  UpdateUI()
    {
        progress.text = $"Câu {currentQuestion + 1}/{totalQuestion}";
        question.text = gameDataList[currentQuestion].question;
        var answers= gameDataList[currentQuestion].answer.Split(',');
        if(answers.Length > 0 )
        {
            question.text = gameDataList[currentQuestion].question;
            var colectObject= collectibles.GetComponentsInChildren<TextMeshProUGUI>();
            if(colectObject.Length>0)
            {
                for(int i=0;i<answers.Length;i++)
                {
                    colectObject[i].text = answers[i];
                }
            }
        }
    }
    public bool CheckQuestion(string answer)
    {
        if (answer == gameDataList[currentQuestion].right_answer)
        {
            return true;
        }
        return false;
    }
    public void NextQuestion()
    {
        currentQuestion++;
        if(currentQuestion>=totalQuestion)
        {
            return;
        }
        else
        {
            UpdateUI();
        }
    }
}

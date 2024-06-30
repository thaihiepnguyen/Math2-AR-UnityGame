using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class QuestionManager : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI progress;
    [SerializeField] TextMeshProUGUI question;
    [SerializeField] GameObject collectibles;
    private List<GameData> gameDataList;
    private GameDTO gameDTO;
    private int currentQuestion=0;
    private int totalQuestion;
    private GameBUS gameBUS = new GameBUS();

    // Start is called before the first frame update
    async void Start()
    {
        var gameResponse= await gameBUS.GetGameDataByLessonId(currentQuestion);
        if(gameResponse.isSuccessful)
        {
            gameDTO = gameResponse.data;
            gameDataList = gameDTO.gameData.ToList();
            totalQuestion = gameDataList.Count();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void UpdateUI()
    {
        progress.text = $"Câu {currentQuestion + 1}/{totalQuestion}";
        question.text = gameDataList[currentQuestion].question;
        var answers= gameDataList[currentQuestion].answer.Split(',');
        if(answers.Length > 0 )
        {

        }
    }
}

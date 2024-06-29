using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileQuizManager : MonoBehaviour
{
    // Start is called before the first frame update



    public GameObject[] choices;
    private QuestionAndAnswers quest;


    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(QuestionAndAnswers quest){
        for (int i = 0; i < 3; i++){
            this.quest = quest;
            choices[i].GetComponentInChildren<TextMeshProUGUI>().text = quest.Answers[i];
        }
    }

    public bool CheckQuiz(string choice){
        
        bool check = choice == quest.Answers[quest.CorrectAnswer-1];
        FindObjectOfType<QuizAnswerManager>().GetComponent<QuizAnswerManager>().UpdateScore(check);
        return check;
    }
}

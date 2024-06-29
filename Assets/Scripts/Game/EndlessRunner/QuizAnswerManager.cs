using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuizAnswerManager : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI choiceTextMesh;
   public List<QuestionAndAnswers> qna;

    private int current;

    public int GetCurrent { get { return current; } }

    private QuestionAndAnswers currentQuest;

    public static int score = 0;

     [SerializeField]  private TextMeshProUGUI scoreText;

      void AddQuestion(string question, string[] answers, int correctAnswer)
    {
        QuestionAndAnswers qa = new()
        {
            Question = question,
            Answers = answers,
            CorrectAnswer = correctAnswer
        };

        qna.Add(qa);
    }
        public static List<QuestionAndAnswers> ShuffleIntList(List<QuestionAndAnswers> list)
    {
        var random = new System.Random();
        var newShuffledList = new List<QuestionAndAnswers>();
        var listCount = list.Count;
        for (int i = 0; i < listCount; i++)
        {
            var randomElementInList = random.Next(0, list.Count);
            newShuffledList.Add(list[randomElementInList]);
            list.Remove(list[randomElementInList]);
        }
        return newShuffledList;
    }
      void Start()
    {
        scoreText.text =  string.Format("Điểm: {0}", score.ToString());
         AddQuestion("2 + 2 = ?", 
            new string[] { "3", 
                "4", 
                "5", 
                 }, 2);

         AddQuestion("3 + 7 = ?", 
            new string[] { "8", 
                "10", 
                "12", 
                 }, 2);
         AddQuestion("4 + 2 = ?", 
            new string[] { "6", 
                "7", 
                "8", 
                 }, 1);

         AddQuestion("10 + 9 = ?", 
            new string[] { "20", 
                "18", 
                "19", 
                 }, 3);

         AddQuestion("9 + 2 = ?", 
            new string[] { "11", 
                "21", 
                "17", 
                 }, 1);

         AddQuestion("6 + 5 = ?", 
            new string[] { "11", 
                "12", 
                "13", 
                 }, 1);
         AddQuestion("9 + 8 = ?", 
            new string[] { "15", 
                "17", 
                "20", 
                 }, 2);

         AddQuestion("10 + 10 = ?", 
            new string[] { "20", 
                "30", 
                "40", 
                 }, 1);

         AddQuestion("6 + 9 = ?", 
            new string[] { "14", 
                "15", 
                "16", 
                 }, 2);
         AddQuestion("9 + 9 = ?", 
            new string[] { "12", 
                "15", 
                "18", 
                 }, 3);

        qna = ShuffleIntList(qna);
        currentQuest = qna[current];
        score = 0;
        current = 0;
    }
  
  
     public bool checkEnd = false;

  

    // Update is called once per frame
    void Update()
    {
         scoreText.text =  string.Format("Điểm: {0}", score.ToString());  

         if (current == qna.Count-1){

            checkEnd = true;

         }
    }
  
   public void SetText(string text){
    choiceTextMesh.SetText(text);
   }

       public void ChangeCurrent(){
        if (current < qna.Count){
            current++;
            currentQuest = qna[current];
        }
    }

    public void UpdateScore(bool correct){

        if (correct){
            score+=1;
        }else {
            GameObject.FindGameObjectWithTag("Player").GetComponent<RunnerController>().ChangeHealth(-1);
        }

        ChangeCurrent();
        choiceTextMesh.gameObject.SetActive(false);


    }

    public void QuizStart(GameObject tile){
        choiceTextMesh.gameObject.SetActive(true);
        SetText(qna[current].Question);
       
        tile.GetComponent<TileQuizManager>().SetText(currentQuest);
        
    }

}

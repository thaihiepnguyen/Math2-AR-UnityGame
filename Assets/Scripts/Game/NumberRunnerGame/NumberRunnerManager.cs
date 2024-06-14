
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.UI;

public class NumberRunnerManager : MonoBehaviour
{
    List<int> numbers = new List<int>();

    [SerializeField] private List<GameObject> spawnObjects = new List<GameObject>();

    private TextMeshProUGUI scoreText;

    private int currentNumber = 0;
    private int currentIndex = 0;

    [SerializeField] GameObject gameOverUI;
     [SerializeField] GameObject gameCompletedUI;

    private bool gameOver = false;
    public bool gameDone {get {return gameOver;} }
    // public int numsCount { get { return numbers.Count;}}

    private int score = 0;
    // public int GetScore { get { return score; }}

    private int currentScore = 0;
    static protected int maxScore = 0;
    public static int GetMaxScore()
    {
        return maxScore;
    }

    
    void Start()
    {
        scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
        scoreText.text =  string.Format("Điểm: {0}", score.ToString());
        for (int i = 0; i< 10; i++){
            numbers.Add(i);
        }

        numbers.Sort();

        currentNumber = numbers[currentIndex];

        for (int i = 0; i < numbers.Count; i++){
            var obj = GetRandomObject();
            if (obj !=null){
            obj.GetComponent<NumberCollectiblesManager>().SetText(numbers[i].ToString());
            
            spawnObjects.Remove(obj);
            }

        
        }

        
    }

    public bool checkEnd = false;

    // Update is called once per frame
    void Update()
    {
         scoreText.text =  string.Format("Điểm: {0}", score.ToString());  

         if (score == numbers.Count){
            checkEnd = true;

         }
    }





     GameObject GetRandomObject()
    {
        return spawnObjects[Random.Range(0, spawnObjects.Count)];
    }


    public void ChangeCurrent(){
        if (currentIndex < numbers.Count){
            currentIndex++;
            currentNumber = numbers[currentIndex];
        }
    }

    public bool CheckCurrent(int index, DragonController controller){

          if(controller!=null)
            {
        if (index != currentNumber){
            

                controller.ChangeHealth(-1);
            return false;
            
        }

        // controller.ChangeHealth(1);
        score+=1;
        ChangeCurrent();


        return true;
            }
        return false;
        
    }



      public void GameOver(){
          GameObject panel = Instantiate(gameOverUI);
          panel.transform.SetParent (GameObject.FindGameObjectWithTag("Canvas").transform, false);
          panel.SetActive(true);
    //     panel.GetComponentInChildren<TextMeshProUGUI>().text = "THUA CUỘC";
    //     panel.GetComponentInChildren<Button>().onClick.AddListener(Restart);
       
           gameOver = true;
        
    }

     public void GameCompleted(){
        var dragon = FindObjectOfType<DragonController>();
        currentScore =  (int) ((dragon.health / (float) dragon.maxHealth) * 100);
        if (currentScore > maxScore){
            maxScore = currentScore;
        }

        GameObject panel = Instantiate(gameCompletedUI);
        panel.transform.SetParent (GameObject.FindGameObjectWithTag("Canvas").transform, false);
        panel.GetComponent<GameCompleteManager>().OnComplete(score.ToString(),maxScore.ToString());

       
        panel.GetComponent<GameCompleteManager>().starAchieved(dragon.health, dragon.maxHealth);
        
        panel.SetActive(true);

        // panel.GetComponentInChildren<Button>().onClick.AddListener(Restart);
       

          gameOver = true;

    }



    
}

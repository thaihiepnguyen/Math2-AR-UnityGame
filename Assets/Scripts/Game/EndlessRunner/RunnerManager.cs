using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool gameOver;

    public static bool gameCompleted;
    public GameObject gameOverPanel;

    public GameObject startingText;
    
    public AutoPlaceOfObjectOnPlane arPlane;

    public static bool isGameStarted;
    void Start()
    {
        gameOver = false;
        gameCompleted = false;
        Time.timeScale = 1;
        isGameStarted = false;
        // startingText.SetActive(false);
   
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver){
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }

        if (gameCompleted){
            Time.timeScale = 0;
        }

        // if (arPlane.doneSpawn){
        //     startingText.SetActive(true);
        // }


        if (SwipeManager.tap && startingText!=null && startingText.activeInHierarchy){
            isGameStarted = true;
            Destroy(startingText);
        }
    }
}

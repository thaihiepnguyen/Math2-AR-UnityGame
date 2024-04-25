using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHistory : MonoBehaviour
{
    private List<string> sceneHistory = new List<string>();  
    
    
    public static SceneHistory instance;


    public static SceneHistory GetInstance(){
        return instance;
    }

      void Awake()
    {
        sceneHistory.Add("Main");
        //  Scene scene = SceneManager.GetActiveScene();

       
        // if (scene.name == "Main")
        // {
        // GameObject.FindGameObjectWithTag("ReturnButton").SetActive(false);
        //  GameObject.FindGameObjectWithTag("HomeButton").SetActive(false);
       
        // }
        // else {
        // GameObject.FindGameObjectWithTag("ReturnButton").SetActive(true);
        //  GameObject.FindGameObjectWithTag("HomeButton").SetActive(true);
        // }
      

        if (instance != null){
          Destroy(gameObject);
          return;
        }

          instance = this;
         DontDestroyOnLoad(gameObject);
    }
  
  
    public void LoadScene(string newScene)
    {
        sceneHistory.Add(newScene);
        SceneManager.LoadScene(newScene);
        
    }
    
    public void OnClickHomeButton(){
        sceneHistory.Clear();
        SceneManager.LoadScene("Main");
    }
   
    public void PreviousScene()
    {
       
        if (sceneHistory.Count >= 1)  //Checking that we have actually switched scenes enough to go back to a previous scene
        {
            sceneHistory.RemoveAt(sceneHistory.Count -1);
            SceneManager.LoadScene(sceneHistory[sceneHistory.Count -1]);
        }
 
    }
}

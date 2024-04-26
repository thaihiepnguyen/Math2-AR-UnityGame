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

        if (instance != null){
            foreach( var x in sceneHistory) {
            Debug.Log( x.ToString());
            }
          Destroy(gameObject);
          return;
        }

          instance = this;
          sceneHistory.Add("Main");
         DontDestroyOnLoad(gameObject);
    }
    

  
    public void LoadScene(string newScene)
    {
        sceneHistory.Add(newScene);
        SceneManager.LoadScene(newScene);
        
    }
    
    public void OnClickHomeButton(){
        sceneHistory.Clear();
        
        LoadScene("Main");
    }
   
    public void PreviousScene()
    {
       
        if (sceneHistory.Count >= 2)  //Checking that we have actually switched scenes enough to go back to a previous scene
        {
            sceneHistory.RemoveAt(sceneHistory.Count -1);
            SceneManager.LoadScene(sceneHistory[sceneHistory.Count -1]);
        }
 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EasyUI.Progress;

public class SceneHistory : MonoBehaviour
{
    private List<string> sceneHistory = new List<string>();


    public static SceneHistory instance;



    public static SceneHistory GetInstance()
    {
        return instance;
    }

    private string lastScene;

    void Awake()
    {
        if (instance != null)
        {
            foreach (var x in sceneHistory)
            {
                Debug.Log(x.ToString());
            }

            Destroy(gameObject);
            return;
        }
        instance = this;
        sceneHistory.Add("Main");
        lastScene = GetPreviousScene();
        DontDestroyOnLoad(gameObject);
    }



    public void LoadScene(string newScene)
    {
        Progress.Hide();
        sceneHistory.Add(newScene);
        SceneManager.LoadScene(newScene);

    }

    public void OnClickHomeButton()
    {
        sceneHistory.Clear();

        LoadScene("Main");
    }

    public void OnClickLogoutButton()
    {
        sceneHistory.Clear();
        PlayerPrefs.DeleteKey("token");
        LoadScene("LoginScene");
    }

    public void PreviousScene()
    {
        Progress.Hide();
        if (sceneHistory.Count >= 2)  //Checking that we have actually switched scenes enough to go back to a previous scene
        {
            lastScene = sceneHistory[sceneHistory.Count - 1];
            sceneHistory.RemoveAt(sceneHistory.Count - 1);
            SceneManager.LoadScene(sceneHistory[sceneHistory.Count - 1]);
        }

    }

    public string GetPreviousScene()
    {
        if (sceneHistory.Count - 2 >= 0)  //Checking that we have actually switched scenes enough to go back to a previous scene
        {
            int nearLastIndex = sceneHistory.Count - 2;
            return sceneHistory[nearLastIndex];
        }
        return sceneHistory[0];
    }

    public string GetLastScene()
    {
        return lastScene;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;
using System;
using UnityEngine.SceneManagement;
public class LessonList : MonoBehaviour
{
    public static string chapterId = "";
    public GameObject lessonPrefab;
    private JSONNode stats;
    private string URL = "http://localhost:3000/lessons/get-lessons-by-chapter-id";
    
    void Start()
    {
       
        StartCoroutine(GetData());
    }

        IEnumerator GetData(){

        
    
        string body = "{\"chapter\": \"" + chapterId + "\"}";
        using (UnityWebRequest request = UnityWebRequest.Post(URL, body, "application/json")){
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            Debug.LogError(request.error);

            else {
                string json = request.downloadHandler.text;
                stats = JSONNode.Parse(json);


               Debug.Log(stats);
                //  TextMeshProUGUI lessonText = lessonPrefab.GetComponentInChildren<TextMeshProUGUI>();
                //  lessonText.text = stats[1]
                foreach (JSONNode node in stats[1])
            {
                GameObject newLesson = Instantiate(lessonPrefab);
                TextMeshProUGUI newLessonText = newLesson.GetComponentInChildren<TextMeshProUGUI>();

                if (newLessonText != null)
                {
                    newLessonText.text = node["name"];// Set button text 

                    
                }

              
                newLesson.transform.SetParent(lessonPrefab.transform.parent.transform, false);
            }  
                
            }
        }
    }
}

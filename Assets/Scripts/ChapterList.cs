using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class ChapterList : MonoBehaviour
{

    public static int semester = 1;
    private JSONNode stats;
    private string URL = "http://localhost:3000/lessons/get-chapter-by-semester/1";
    [SerializeField] private TextMeshProUGUI first;
    [SerializeField] private TextMeshProUGUI second;
    [SerializeField] private TextMeshProUGUI third;

    [SerializeField] private TextMeshProUGUI title;
    // public int CurrentSemester {get { return semester; } set { semester = value; }}
    // Start is called before the first frame update
    void Start()
    {
        if (semester == 2){
             URL = "http://localhost:3000/lessons/get-chapter-by-semester/2";
            
        }

        StartCoroutine(GetData());
    }

        IEnumerator GetData(){
        using (UnityWebRequest request = UnityWebRequest.Get(URL)){
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            Debug.LogError(request.error);

            else {
                string json = request.downloadHandler.text;
                stats = JSONNode.Parse(json);


                title.text = String.Format("HỌC KÌ {0}",semester);
                 string converter = stats[1][0][0];

                
                first.text = String.Format("Chương {0}",converter[0]);

                converter = stats[1][1][0];
                second.text = String.Format("Chương {0}",converter[0]);

                converter = stats[1][2][0];
                third.text = String.Format("Chương {0}",converter[0]);
                
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

     public void FirstChapter(){
    Chapter.title = stats[1][0][0];
       SceneManager.LoadScene("ChapterScene");
    }

     public void SecondChapter(){
     Chapter.title = stats[1][1][0];
        SceneManager.LoadScene("ChapterScene");
    }

     public void ThirdChapter(){
       Chapter.title = stats[1][2][0];
       SceneManager.LoadScene("ChapterScene");
    }


}

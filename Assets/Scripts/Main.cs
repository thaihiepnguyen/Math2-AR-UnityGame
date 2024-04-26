using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Main : MonoBehaviour
{
    public void OnClickPlay(){
        SceneHistory.GetInstance().LoadScene("Semester");
    }

    public void OnClickAchievementButton(){
        SceneHistory.GetInstance().LoadScene("Achievement");
    }
}

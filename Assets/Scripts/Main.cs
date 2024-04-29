using UnityEngine;
public class Main : MonoBehaviour
{
    public void OnClickPlay(){
        SceneHistory.GetInstance().LoadScene("Semester");
    }

    public void OnClickAchievementButton(){
        SceneHistory.GetInstance().LoadScene("Achievement");
    }

    public void OnClickStore(){
        SceneHistory.GetInstance().LoadScene("Store");
    }
}

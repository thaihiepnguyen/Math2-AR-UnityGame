using UnityEngine;
public class Main : MonoBehaviour
{
    
    [SerializeField] private GameObject prototype;
    [SerializeField] private GameObject container;

    async void Start()
    {
        var userBus = new UserBUS();
        var response = await userBus.GetMe();
        PlayerPrefs.SetInt("uid", 1);
        if (response.isSuccessful){
            StartCoroutine(LoadModelManager.LoadModel(response.data.three_dimension_id,prototype,container,true));
        }
    }

    public void OnClickPlay(){
        SceneHistory.GetInstance().LoadScene("Semester");
    }

    public void OnClickAchievementButton(){
        SceneHistory.GetInstance().LoadScene("Achievement");
    }

    public void OnClickStore(){
        SceneHistory.GetInstance().LoadScene("Store");
    }

    // public void OnClickPersonalButton() {
    //     SceneHistory.GetInstance().LoadScene("PersonalScene");
    // }
}

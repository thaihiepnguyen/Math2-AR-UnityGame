using UnityEngine;
public class Main : MonoBehaviour
{
    
    [SerializeField] private GameObject prototype;

    async void Start()
    {
        var userBus = new UserBUS();
        var response = await userBus.GetMe();
        if (response.isSuccessful){
            Debug.Log("Successful");
               StartCoroutine(LoadModelManager.LoadModel(response.data.three_dimension_id,prototype));
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

using UnityEngine;
public class Main : MonoBehaviour
{
    
    [SerializeField] private GameObject prototype;
    [SerializeField] private GameObject container;

    async void Start()
    {
        var userBus = new UserBUS();
        var response = await userBus.GetMe();
        if (response.isSuccessful){
            Debug.Log("Successful");
               StartCoroutine(LoadModelManager.LoadModel(response.data.three_dimension_id,prototype,container));
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

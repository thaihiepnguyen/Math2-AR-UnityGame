using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PanelManager : MonoBehaviour
{
     [SerializeField]
    private GameObject container;

       [SerializeField]
    private Image _skinImage;

    [SerializeField]
    private Image _frameImage;

    [SerializeField]
    private Button _logoutBtn;

    [SerializeField]
    private Button _settingBtn;

    [SerializeField]
    private TextMeshProUGUI _totalOfAchievement;

    [SerializeField]
    private TextMeshProUGUI _totalOfNote;

    [SerializeField]
    private TextMeshProUGUI _username;

     [SerializeField]
    private GameObject dragon;

   
     async void Awake(){
       

          UserBUS userBUS = new();
        var response = await userBUS.GetProfile();
        if (response.isSuccessful) {
            PersonalDTO personalDTO = response.data;
            if (personalDTO.skinUrl != null) {
                StartCoroutine(LoadImageManager.LoadImage(_skinImage, personalDTO.skinUrl));
            }
            _username.text = personalDTO.username;
            _totalOfAchievement.text = personalDTO.totalAchievement.ToString();
            _totalOfNote.text = personalDTO.totalNote.ToString();

            // _logoutBtn.onClick.AddListener(OnLogoutCLick);

           
        }
    }

    // Start is called before the first frame update
 
    public void OnEnable()
    {
         LeanTween.reset();
        float width = container.gameObject.GetComponent<RectTransform>().rect.width;

        // container.gameObject.GetComponent<Image>().enabled = true;
        dragon.SetActive(false);
        Debug.Log(width);
         LeanTween.moveLocalX(gameObject, width/2, 0.5f).setEase(LeanTweenType.easeInOutCirc);
          
    }


    public void Close(){
        LeanTween.reset();
         float width = container.gameObject.GetComponent<RectTransform>().rect.width;

       
        Debug.Log(width);
         LeanTween.moveLocalX(gameObject, -width/2f, 0.5f).setEase(LeanTweenType.easeInOutCirc).setOnComplete(()=>{
             dragon.SetActive(true);
            this.gameObject.SetActive(false);
         });
        //  this.gameObject.SetActive(false);

         Debug.Log("Hello");
          
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
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
        LeanTween.reset();

          UserBUS userBUS = new();
        var response = await userBUS.GetProfile();
        if (response.isSuccessful) {
            PersonalDTO personalDTO = response.data;
            if (personalDTO.imageSkinId != null) {
                StartCoroutine(LoadImageManager.LoadBinaryImage(_skinImage, personalDTO.imageSkinId));
            }
            _username.text = personalDTO.username;
            _totalOfAchievement.text = personalDTO.totalAchievement.ToString();
            _totalOfNote.text = personalDTO.totalNote.ToString();

            _logoutBtn.onClick.AddListener(OnLogoutCLick);
        }
    }

    // Start is called before the first frame update
 
    public void OnEnable()
    {
        float width = container.gameObject.GetComponent<RectTransform>().rect.width;

        container.gameObject.GetComponent<Image>().enabled = true;
        dragon.SetActive(false);
        Debug.Log(width);
         LeanTween.moveLocalX(gameObject, -width/2f, 0.5f).setEase(LeanTweenType.easeInOutCirc);
    }

     void OnLogoutCLick() {
        SceneHistory.GetInstance().OnClickLogoutButton();
    }
}

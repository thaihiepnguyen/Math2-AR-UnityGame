using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;




public class PersonalManager : MonoBehaviour
{
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
    // Start is called before the first frame update
    async void Start()
    {
        UserBUS userBUS = new();
        var response = await userBUS.GetProfile();
        if (response.isSuccessful) {
            PersonalDTO personalDTO = response.data;
            if (personalDTO.skinUrl != null) {
                StartCoroutine(LoadImageManager.LoadImage(this._skinImage, personalDTO.skinUrl));
            }
            this._username.text = personalDTO.username;
            this._totalOfAchievement.text = personalDTO.totalAchievement.ToString();
            this._totalOfNote.text = personalDTO.totalNote.ToString();

            this._logoutBtn.onClick.AddListener(OnLogoutCLick);
        }
    }

    void OnLogoutCLick() {
        SceneHistory.GetInstance().OnClickLogoutButton();
    }
}

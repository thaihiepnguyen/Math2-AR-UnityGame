using EasyUI.Progress;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class LoginScreenManager : MonoBehaviour
{
    

    public TMP_InputField EmailField;
    public TMP_InputField PasswordField;
    public Button LoginButton;
    public Button RegisterButton;
    public Button OkButton;
    public Canvas errorCanvas;

    // Start is called before the first frame update
    void Start()
    {
        PasswordField.contentType = TMP_InputField.ContentType.Password;

        LoginButton.onClick.AddListener(OnClickLogin);
        RegisterButton.onClick.AddListener(OnClickRegister);
        OkButton.onClick.AddListener(OnClickOk);
        HideErrorCanvas();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async void OnClickLogin()
    {
        var email = EmailField.text;
        var password = PasswordField.text;

        Progress.Show("Đang xử lý...", ProgressColor.Orange);

        var loginBus = new AuthBUS();
        var response = await loginBus.LoginByEmail(new LoginEmailDTO
        {
            email = email,
            password = password
        });
        
        if (response.isSuccessful)
        {
            Debug.Log("Login Successful");
            // Set uid 
            PlayerPrefs.SetInt("uid", response.data);
           
            Progress.Hide();
            SceneManager.LoadScene(GlobalVariable.MAIN_SCENE);
        }
        else
        {
            Debug.Log($"Login Failed: {response.message}");
            Progress.Hide();
            ShowErrorCanvas();
        }
    }

    void OnClickRegister()
    {
        Debug.Log("Register Button Clicked");
        SceneManager.LoadScene(GlobalVariable.REGISTER_SCENE);
    }
    void ShowErrorCanvas()
    {
        errorCanvas.gameObject.SetActive(true);
    }

    void HideErrorCanvas()
    {
        errorCanvas.gameObject.SetActive(false);
    }

    public void OnClickOk()
    {
        HideErrorCanvas();
    }
}

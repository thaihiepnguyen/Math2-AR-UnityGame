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

        var loginBus = new AuthBUS();
        var response = await loginBus.LoginByEmail(new LoginDTO
        {
            email = email,
            password = password
        });
        
        if (response.isSuccessful)
        {
            Debug.Log("Login Successful");
            // Set uid 
            
            Debug.Log("Please take this uid in main screen to get profile of user" + response.data);
            PlayerPrefs.SetInt("uid", response.data);
            
            // To explain how to use the uid in the future
            var userBus = new UserBUS();
            var me = await userBus.GetUserById(PlayerPrefs.GetInt("uid"));

            if (me.isSuccessful)
            {
                Debug.Log(me.data.email);
            }
            
            SceneManager.LoadScene("Main");
        }
        else
        {
            Debug.Log($"Login Failed: {response.message}");
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

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EasyUI.Progress;

public class RegisterScreenManager : MonoBehaviour
{
    public TMP_InputField EmailField;
    public TMP_InputField PasswordField;
    public TMP_InputField ConfirmPasswordField;

    public Button LoginButton;
    public Button RegisterButton;

    public Text ErrorMessage;
    
    public Button OkButton;
    public Button ReturnButton;
    public Canvas errorCanvas;
    public Canvas successCanvas;

    // Start is called before the first frame update
    void Start()
    {
        PasswordField.contentType = TMP_InputField.ContentType.Password;
        ConfirmPasswordField.contentType = TMP_InputField.ContentType.Password;

        LoginButton.onClick.AddListener(OnClickLogin);
        RegisterButton.onClick.AddListener(OnClickRegister);
        OkButton.onClick.AddListener(OnClickOk);
        ReturnButton.onClick.AddListener(OnClickReturn);
        HideErrorCanvas();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClickLogin()
    {
        Debug.Log("Login Button Clicked");
        SceneManager.LoadScene(GlobalVariable.LOGIN_SCENE);
    }

    async void OnClickRegister()
    {
        string email = EmailField.text;
        string password = PasswordField.text;
        string confirmPassword = ConfirmPasswordField.text;

        if (email == "" || email == null)
        {
            ShowErrorCanvas("Email không được để trống");
        }else if (password == "" || password == null)
        {
            ShowErrorCanvas("Mật khẩu không được trống");
        } else if (password != confirmPassword)
        {
            ShowErrorCanvas("Mật khẩu xác nhận không đúng");
        }
        else
        {
            Progress.Show("Đang xử lý...", ProgressColor.Orange);
            var loginBus = new AuthBUS();

            var response = await loginBus.RegisterByEmail(new RegisterDTO()
            {
                email = email,
                password = password
            });

            if (response.isSuccessful)
            {
                Debug.Log("Register Successful");
                Progress.Hide();
                ShowSuccessCanvas();
            }
            else
            {
                Debug.Log("Register Failed: " + response.message);
                Progress.Hide();
                ShowErrorCanvas("Tài khoản đã tồn tại");
            }
        }
    }

    void ShowErrorCanvas(string message)
    {
        ErrorMessage.text = message;
        errorCanvas.gameObject.SetActive(true);
    }

    void HideErrorCanvas()
    {
        errorCanvas.gameObject.SetActive(false);
    }

    void ShowSuccessCanvas()
    {
        successCanvas.gameObject.SetActive(true);
    }

    void HideSuccessCanvas()
    {
        successCanvas.gameObject.SetActive(false);
    }

    public void OnClickOk()
    {
        HideErrorCanvas();
    }

    public void OnClickReturn()
    {
        HideSuccessCanvas();
        SceneManager.LoadScene(GlobalVariable.LOGIN_SCENE);
    }
}

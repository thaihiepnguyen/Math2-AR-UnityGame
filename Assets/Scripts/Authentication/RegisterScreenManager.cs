using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RegisterScreenManager : MonoBehaviour
{
    public TMP_InputField EmailField;
    public TMP_InputField PasswordField;
    public TMP_InputField ConfirmPasswordField;

    public Button LoginButton;
    public Button RegisterButton;

    public Text ErrorMessage;
    
    public Button OkButton;
    public Canvas errorCanvas;

    private string registerUrl = "http://localhost:3000/register-by-email";

    // Start is called before the first frame update
    void Start()
    {
        PasswordField.contentType = TMP_InputField.ContentType.Password;
        ConfirmPasswordField.contentType = TMP_InputField.ContentType.Password;

        LoginButton.onClick.AddListener(OnClickLogin);
        RegisterButton.onClick.AddListener(OnClickRegister);
        OkButton.onClick.AddListener(OnClickOk);
        HideErrorCanvas();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClickLogin()
    {
        Debug.Log("Login Button Clicked");
        SceneManager.LoadScene("LoginScene");
    }

    void OnClickRegister()
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
            StartCoroutine(RegisterRoutine(email, password));
        }
    }

    IEnumerator RegisterRoutine(string email, string password)
    {
        Debug.Log("Register Button Clicked");

        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);

        // Send the request
        using (UnityWebRequest www = UnityWebRequest.Post(registerUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Register Successful");
                SceneManager.LoadScene("Login");
            }
            else
            {
                Debug.Log("Register Failed: " + www.error);
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

    public void OnClickOk()
    {
        HideErrorCanvas();
    }
}

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

    private string loginUrl = "http://localhost:3000/account/login-by-email";

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

    void OnClickLogin()
    {
        string email = EmailField.text;
        string password = PasswordField.text;

        Progress.Show("Đang xử lý...", ProgressColor.Orange);
        StartCoroutine(LoginRoutine(email, password));
    }

    IEnumerator LoginRoutine(string email, string password)
    {
        Debug.Log("Login Button Clicked");

        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);

        // Send the request
        using (UnityWebRequest www = UnityWebRequest.Post(loginUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Login Successful");
                Progress.Hide();
                SceneManager.LoadScene(GlobalVariable.MAIN_SCENE);
            }
            else
            {
                Debug.Log("Login Failed: " + www.error);
                Progress.Hide();
                ShowErrorCanvas();
            }
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

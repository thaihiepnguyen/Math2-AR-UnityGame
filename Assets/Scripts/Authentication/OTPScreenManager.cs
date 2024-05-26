using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EasyUI.Progress;
using Firebase.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class OTPScreenManager : MonoBehaviour
{
    public TMP_InputField OTPField;
    public Canvas errorCanvas;
    public Canvas successCanvas;
    public Text ErrorMessage;
    public Button OkButton;
    public Button ReturnButton;
    public Button ConfirmButton;

    // Start is called before the first frame update
    async void Start()
    {
        ConfirmButton.onClick.AddListener(VerifyOTP);
        OkButton.onClick.AddListener(OnClickOk);
        ReturnButton.onClick.AddListener(OnClickReturn);
        HideErrorCanvas();
        await SignInWithPhoneNumber(PlayerPrefs.GetString("email"));
    }

    public async void VerifyOTP()
    {
        bool isError = false;
        string error = "";
        Progress.Show("Đang xử lý...", ProgressColor.Orange);
        PhoneAuthCredential credential =
        GlobalVariable.provider.GetCredential(GlobalVariable.VerificationId, OTPField.text);
        await GlobalVariable.auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWith(async task =>
        {
            if(task.IsCompletedSuccessfully){
                FirebaseUser newUser = task.Result.User;
                Debug.Log(newUser.PhoneNumber);
                var loginBus = new AuthBUS();
                var response = await loginBus.VerifyByPhone(new RegisterPhoneDTO
                {
                    phone = newUser.PhoneNumber,
                    password = PlayerPrefs.GetString("password"),
                });
                if (response.isSuccessful)
                {
                    Debug.Log("get here");
                    var responseLogin = await loginBus.LoginByEmail(new LoginEmailDTO
                    {
                        email = newUser.PhoneNumber,
                        password = PlayerPrefs.GetString("password")
                    });
                    PlayerPrefs.SetString("password", "");
                    if (responseLogin.isSuccessful)
                    {
                        PlayerPrefs.SetInt("uid", responseLogin.data);
                        Progress.Hide();
                        isError = false;
                        // ShowSuccessCanvas();
                    }
                    else{
                        Progress.Hide();
                        isError = true;
                        error = "Đăng nhập không thành công";
                        // ShowErrorCanvas("Đăng nhập không thành công");
                    }
                }
                else{
                    Progress.Hide();
                    isError = true;
                    error = "Tài khoản đã được xác thực";
                    // ShowErrorCanvas("Tài khoản đã được xác thực");
                }
            }
            else
            {
                // Debug.LogError("SignInAndRetrieveDataWithCredentialAsync encountered an error: " +
                //             task.Exception);
                Progress.Hide();
                isError = true;
                error = "OTP không chính xác";
                // ShowErrorCanvas("OTP không chính xác");
                // Debug.Log("get here");
                // Thread.Sleep(2000);
            }
        });
        if(isError) ShowErrorCanvas(error);
        else    ShowSuccessCanvas();
    }

    private async Task SignInWithPhoneNumber(string phoneNumber){
        GlobalVariable.provider = PhoneAuthProvider.GetInstance(GlobalVariable.auth);
        GlobalVariable.provider.VerifyPhoneNumber(
        new Firebase.Auth.PhoneAuthOptions {
            PhoneNumber = phoneNumber,
            TimeoutInMilliseconds = GlobalVariable.phoneAuthTimeoutMs,
            ForceResendingToken = null
        },
        verificationCompleted: (credential) => {
            // Auto-sms-retrieval or instant validation has succeeded (Android only).
            // There is no need to input the verification code.
            // `credential` can be used instead of calling GetCredential().
        },
        verificationFailed: (error) => {
            // The verification code was not sent.
            // `error` contains a human readable explanation of the problem.
        },
        codeSent: (id, token) => {
            GlobalVariable.VerificationId = id;
            // Verification code was successfully sent via SMS.
            // `id` contains the verification id that will need to passed in with
            // the code from the user when calling GetCredential().
            // `token` can be used if the user requests the code be sent again, to
            // tie the two requests together.
        },
        codeAutoRetrievalTimeOut: (id) => {
            // Called when the auto-sms-retrieval has timed out, based on the given
            // timeout parameter.
            // `id` contains the verification id of the request that timed out.
        });
    }

    // Update is called once per frame
    void Update()
    {
        
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
    public async void OnClickReturn()
    {
        HideSuccessCanvas();
        SceneManager.LoadScene(GlobalVariable.MAIN_SCENE);
    }
}

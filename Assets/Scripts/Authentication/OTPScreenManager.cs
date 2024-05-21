using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OTPScreenManager : MonoBehaviour
{
    public TMP_InputField OTPField;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void VerifyOTP()
    {
        PhoneAuthCredential credential =
        GlobalVariable.provider.GetCredential(GlobalVariable.VerificationId, OTPField.text);
        GlobalVariable.auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWith(async task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync encountered an error: " +
                            task.Exception);
                return;
            }
            FirebaseUser newUser = task.Result.User;
            Debug.Log(newUser.PhoneNumber);
            var loginBus = new AuthBUS();
            var response = await loginBus.VerifyByPhone(new RegisterPhoneDTO
            {
                phone = newUser.PhoneNumber,
                password = PlayerPrefs.GetString("password"),
            });
            Debug.Log(response.isSuccessful);
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
                    SceneManager.LoadScene(GlobalVariable.MAIN_SCENE);
                }
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

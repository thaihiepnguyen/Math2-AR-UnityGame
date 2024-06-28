using EasyUI.Progress;
using Firebase;
using Firebase.Auth;
using Google;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
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
    // private FirebaseAuth auth;
    private GoogleSignInConfiguration configuration;
    public TextMeshProUGUI infoText;
    public string webClientId = "<your client id here>";

    public TMP_InputField OTPField;

    public Toggle RememberMe;


    private async  void Awake()
    {
        var userBus = new UserBUS();
        string token = PlayerPrefs.GetString("token");
        if (!token.Equals("")) {
            API.AddToken(token);   
            var response = await userBus.GetMe();
            if (response.isSuccessful) {
                SceneManager.LoadScene(GlobalVariable.MAIN_SCENE);
            }     
        }
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
        CheckFirebaseDependencies();
    }


    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_ANDROID
        GlobalVariable.platform = 1;
        #elif UNITY_IOS
            GlobalVariable.platform = 2;
        #endif
    
        PasswordField.contentType = TMP_InputField.ContentType.Password;


        LoginButton.onClick.AddListener(OnClickLogin);
        RegisterButton.onClick.AddListener(OnClickRegister);
        OkButton.onClick.AddListener(OnClickOk);
        HideErrorCanvas();
    }

    private void CheckFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result == DependencyStatus.Available)
                    GlobalVariable.auth = FirebaseAuth.DefaultInstance;
                else
                    AddToInformation("Could not resolve all Firebase dependencies: " + task.Result.ToString());
            }
            else
            {
                AddToInformation("Dependency check was not completed. Error : " + task.Exception.Message);
            }
        });
    }

    public void SignInWithGoogle() { OnSignIn(); }
    public void SignOutFromGoogle() { OnSignOut(); }

    private void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddToInformation("Calling SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    private void OnSignOut()
    {
        AddToInformation("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }

    public void OnDisconnect()
    {
        AddToInformation("Calling Disconnect");
        GoogleSignIn.DefaultInstance.Disconnect();
    }

    internal async void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    AddToInformation("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    AddToInformation("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            AddToInformation("Canceled");
        }
        else
        {
            // AddToInformation("Welcome: " + task.Result.DisplayName + "!");
            // AddToInformation("Email = " + task.Result.Email);
            // AddToInformation("Google ID Token = " + task.Result.IdToken);
            // AddToInformation("Email = " + task.Result.Email);
            string idToken = task.Result.IdToken;
            SignInWithGoogleOnFirebase(idToken);
            // Progress.Show("Đang xử lý...", ProgressColor.Orange);
            var loginBus = new AuthBUS();
            var response = await loginBus.LoginExternalParty(new LoginExternalDTO
            {
                email = task.Result.Email,
                uid = task.Result.UserId,
                token = task.Result.IdToken,
                platform = "Google",
            });
            if (response.isSuccessful)
            {
                SceneManager.LoadScene(GlobalVariable.MAIN_SCENE);

            }
            else
            {
                Debug.Log($"Login Failed: {response.message}");
                ShowErrorCanvas();
            }
        }
    }
    private void SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        GlobalVariable.auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {

            AggregateException ex = task.Exception;
            if (ex != null)
            {

                if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                    AddToInformation("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
            }
            else
            {
                AddToInformation("Sign In Successful.");
            }

        });

    }

    public void OnSignInSilently()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddToInformation("Calling SignIn Silently");

        GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnAuthenticationFinished);
    }

    public void OnGamesSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;

        AddToInformation("Calling Games SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    // public void VerifyOTP(){
    //     PhoneAuthCredential credential =
    //     GlobalVariable.provider.GetCredential(VerificationId, OTPField.text);
    //     GlobalVariable.auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWith(task => {
    //         if (task.IsFaulted) {
    //             Debug.LogError("SignInAndRetrieveDataWithCredentialAsync encountered an error: " +
    //                         task.Exception);
    //             return;
    //         }
    //         FirebaseUser newUser = task.Result.User;
    //         Debug.Log("User signed in successfully");
    //         // This should display the phone number.
    //         Debug.Log("Phone number: " + newUser.PhoneNumber);
    //         // The phone number providerID is 'phone'.
    //         Debug.Log("Phone provider ID: " + newUser.ProviderId);
    //     });
    // }

    // Update is called once per frame
    void Update()
    {
        
    }

    async void OnClickLogin()
    {
        GlobalVariable.IS_REMEMBER_ME = RememberMe.isOn;
        var email = EmailField.text;
        var password = PasswordField.text;
        var isNumeric = Regex.IsMatch(email,@"^(\+[\d]{1,5}|0)?[0-9]\d{8}$");
        if(isNumeric && email[0] == '0')   email = "+84" + email[1..];   
        Progress.Show("Đang xử lý...", ProgressColor.Orange);

        var loginBus = new AuthBUS();
        var response = await loginBus.LoginByEmail(new LoginEmailDTO
        {
            email = email,
            password = password,
            rememberMe = RememberMe.isOn
        });
        
        if (response.isSuccessful)
        {
        
            Progress.Hide();
            SceneManager.LoadScene(GlobalVariable.MAIN_SCENE);
        }
        else if(isNumeric && string.Equals(response.message,"email not verified")){
            PlayerPrefs.SetString("email", email);
            PlayerPrefs.SetString("password", password);
            Progress.Hide();
            SceneManager.LoadScene(GlobalVariable.OTP_SCENE);
        } else
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
    private void AddToInformation(string str) { infoText.text += "\n" + str; }
}

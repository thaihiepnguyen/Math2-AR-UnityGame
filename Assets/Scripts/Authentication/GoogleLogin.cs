using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyUI.Progress;
using Firebase;
using Firebase.Auth;
using Google;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Facebook.Unity;
using Unity.VisualScripting;


public class GoogleLogin : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    public string webClientId = "<your client id here>";

    private FirebaseAuth auth;
    private GoogleSignInConfiguration configuration;
    public Canvas errorCanvas;


    private void Awake()
    {
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
        CheckFirebaseDependencies();
        if (!FB.IsInitialized){
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else{
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    //FB Login

    private void InitCallback(){
        if (FB.IsInitialized){
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else{
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown){
        if (!isGameShown){
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else{
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    public void FBlogin(){
        // var permissions = new List<string>().{
        //     "email",
        //     "public_profile",
        //     "user_friends",
        // };
        List<string> permissions = new List<string>()
        {
            "email",
            "public_profile",
            "user_friends"
        };
        FB.LogInWithReadPermissions(permissions, AuthCallback);
    
    }
    private async void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {   
            var aToken = AccessToken.CurrentAccessToken;

            IUserInfo user = null;
            Credential credential = FacebookAuthProvider.GetCredential(aToken.TokenString);
            await auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithCredentialAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                    return;
                }
                var temp = task.Result.ProviderData.AsReadOnlyCollection().ToArray();
                user = temp[0];
            });
            if(user != null){
                var loginBus = new AuthBUS();
                Debug.Log("uid: " + user.UserId);
                var response = await loginBus.LoginExternalParty(new LoginExternalDTO
                {
                    email = user.Email,
                    uid = user.UserId,
                    token = aToken.TokenString,
                    platform = "Facebook",
                });
                if (response.isSuccessful)
                {
                    // // Set uid 
                    // Debug.Log("Please take this uid in main screen to get profile of user" + response.data);
                    PlayerPrefs.SetInt("uid", response.data);

                    // To explain how to use the uid in the future
                    var userBus = new UserBUS();
                    var me = await userBus.GetUserById(PlayerPrefs.GetInt("uid"));

                    if (me.isSuccessful)
                    {
                        Debug.Log(me.data.email);
                    }

                    SceneManager.LoadScene(GlobalVariable.MAIN_SCENE);
                }
                else
                {
                    Debug.Log($"Login Failed: {response.message}");
                    ShowErrorCanvas();
                }
            }
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }


    private void CheckFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result == DependencyStatus.Available)
                    auth = FirebaseAuth.DefaultInstance;
                else
                    Debug.Log("Could not resolve all Firebase dependencies: " + task.Result.ToString());
            }
            else
            {
                Debug.Log("Dependency check was not completed. Error : " + task.Exception.Message);
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
        Debug.Log("Calling SignIn");

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
                    Debug.Log("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    Debug.Log("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            Debug.Log("Canceled");
        }
        else
        {
            // AddToInformation("Welcome: " + task.Result.DisplayName + "!");
            // AddToInformation("Email = " + task.Result.Email);
            // AddToInformation("Google ID Token = " + task.Result.IdToken);
            // AddToInformation("Email = " + task.Result.Email);
            string idToken = task.Result.IdToken;
            await SignInWithGoogleOnFirebase(idToken);
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
                // // Set uid 
                // Debug.Log("Please take this uid in main screen to get profile of user" + response.data);
                PlayerPrefs.SetInt("uid", response.data);

                // To explain how to use the uid in the future
                var userBus = new UserBUS();
                var me = await userBus.GetUserById(PlayerPrefs.GetInt("uid"));

                if (me.isSuccessful)
                {
                    Debug.Log(me.data.email);
                }

                SceneManager.LoadScene(GlobalVariable.MAIN_SCENE);
            }
            else
            {
                Debug.Log($"Login Failed: {response.message}");
                ShowErrorCanvas();
            }
        }
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

    private async Task SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        await auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {

            AggregateException ex = task.Exception;
            if (ex != null)
            {

                if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                    Debug.Log("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
            }
            else
            {
                Debug.Log("Sign In Successful.");
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

    private void AddToInformation(string str) { infoText.text += "\n" + str; }

}

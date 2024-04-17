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
            var response = await loginBus.LoginExternalParty(new LoginExternalDTO{
                email = task.Result.Email,
                uid = task.Result.UserId,
                token = task.Result.IdToken,
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
                    AddToInformation(me.data.email);
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

    private void SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
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

    private void AddToInformation(string str) { infoText.text += "\n" + str; }

}

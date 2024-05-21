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
    public TMP_InputField PhoneNumberField;
    public TMP_InputField OTPField;

    public string webClientId = "<your client id here>";
    PhoneAuthProvider provider;
    private uint phoneAuthTimeoutMs = 60 * 1000;
    private string VerificationId;
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

    public void SignInWithPhoneNumber(string phoneNumber){
        provider = PhoneAuthProvider.GetInstance(auth);
        provider.VerifyPhoneNumber(
        new Firebase.Auth.PhoneAuthOptions {
            PhoneNumber = phoneNumber,
            TimeoutInMilliseconds = phoneAuthTimeoutMs,
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
            VerificationId = id;
            Debug.Log(PhoneNumberField.text);
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

    public void VerifyOTP(){
        PhoneAuthCredential credential =
        provider.GetCredential(VerificationId, OTPField.text);
        auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsFaulted) {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync encountered an error: " +
                            task.Exception);
                return;
            }
            FirebaseUser newUser = task.Result.User;
            Debug.Log("User signed in successfully");
            // This should display the phone number.
            Debug.Log("Phone number: " + newUser.PhoneNumber);
            // The phone number providerID is 'phone'.
            Debug.Log("Phone provider ID: " + newUser.ProviderId);
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

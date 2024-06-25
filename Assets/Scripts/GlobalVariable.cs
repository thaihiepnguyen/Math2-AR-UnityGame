using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;

public class GlobalVariable
{

    public static int  platform =  1;
    public const string MAIN_SCENE = "Main";
    public const int LOGIN_SCENE = 0;
    public const int REGISTER_SCENE = 2;
    public const string EXERCISES_SCENE = "Exercises";
    public const string OTP_SCENE = "OTPScene";
    
    public static FirebaseAuth auth;
    public static PhoneAuthProvider provider;
    public static string VerificationId;
    public static uint phoneAuthTimeoutMs = 60 * 1000;
    // public static int currentAmount;
    public const string server_url = "http://192.168.100.10:3000";
    public const string DragDropType = "DragDrop";
    public const string MULTIPLE_CHOICE_TYPE = "MultipleChoice";
    public const string InputType = "Input";
    public const string MultipleChoiceType = "ABCD";

    public const string INPUT_TYPE = "Input";
    //PlayerPref variable string
    public const string userID = "uid";
    //Scene number
    public const string EXAM_SCENE = "ExamScene";
    public const string EXAM_LIST_SCENE = "ExamListScene";

    public static bool IS_REMEMBER_ME = false;
}

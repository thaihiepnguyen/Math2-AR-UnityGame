using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;

public class GlobalVariable
{
    //SCENE NAME
    public const string MAIN_SCENE = "Main";
    public const string EXERCISES_SCENE = "Exercises";
    public const string OTP_SCENE = "OTPScene";
    public const int LOGIN_SCENE = 0;
    public const int REGISTER_SCENE = 2;
    public const string EXAM_SCENE = "ExamScene";
    public const string EXAM_LIST_SCENE = "ExamListScene";
    public const string BOOK_LEARNING_SCENE = "BookLearningScene";
    public const string VIDEO_LEARNING_SCENE = "VideoLearning";
    public const string LESSON_LIST_SCENE = "LessonList";
    public const string NOTES_SCENE = "NotesScene";

    //STRING CONSTANT
    public const string DragDropType = "DragDrop";
    public const string MULTIPLE_CHOICE_TYPE = "MultipleChoice";
    public const string InputType = "Input";
    public const string INPUT_TYPE = "Input";
    public const string MultipleChoiceType = "ABCD";
    public const string userID = "uid";
    public const string LOADIND_TEXT = "ĐANG TẢI";

    //SERVICE CONFIG
    public const string server_url = "http://localhost:3000";
    public static int  platform =  1;
    public static FirebaseAuth auth;
    public static PhoneAuthProvider provider;
    public static string VerificationId;
    public static uint phoneAuthTimeoutMs = 60 * 1000;
    public static bool IS_REMEMBER_ME = false;
}

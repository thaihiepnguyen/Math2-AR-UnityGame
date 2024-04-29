using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public partial class ReviewManager : MonoBehaviour
{
    [SerializeField]
    GameObject d_questionList;
    // Question Title
    //Answer List
    [SerializeField]
    GameObject d_answerList;
    //Result List
    [SerializeField]
    GameObject d_result;
    [SerializeField]
    GameObject d_answerSlot;
    public Sprite correctSprite;
    public Sprite incorrectSprite;
    void ChangeDragDropObject( ExerciseDTO exercise, QuestionResultDTO questionResult)
    {
        var questions = exercise.question.Split(",");
        var answers = questionResult.user_answer.Split(",");
        var right_answers=exercise.right_answer.Split(",");

        var aslot=d_answerSlot.GetComponentsInChildren<TextMeshProUGUI>();
        var a = d_answerList.GetComponentsInChildren<TextMeshProUGUI>();

        var q = d_questionList.GetComponentsInChildren<TextMeshProUGUI>();
        var resultImageList= d_result.GetComponentsInChildren<Image>();
        for (int i = 0; i < a.Length; i++)
        {
            a[i].text = right_answers[i];
            q[i].text = questions[i];
            aslot[i].text = answers[i];
            if (answers[i] == right_answers[i])
            {
                resultImageList[i].sprite= correctSprite;
            }
            else
            {
                resultImageList[i].sprite = incorrectSprite;
            }
        }
    }
}

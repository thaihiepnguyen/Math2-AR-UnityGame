using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public partial class ReviewManager : MonoBehaviour
{
    [SerializeField] private TMP_Text i_question;

    [SerializeField] private TMP_InputField i_answer;

    [SerializeField] private GameObject i_holder;
    [SerializeField] private Image i_result;
    [SerializeField] private TextMeshProUGUI i_rightAnswer;
    public void ChangeInputObject(ExerciseDTO exercise, QuestionResultDTO questionResult)
    {
        var question = exercise.question;
        var right_answers = exercise.right_answer.Split(",");
        Debug.Log(right_answers.Length);

        i_question.text = question;
        TMP_InputField[] inputList = i_holder.GetComponentsInChildren<TMP_InputField>();
        if (right_answers.Length > inputList.Length)
        {

            for (int i = 1; i < right_answers.Length; i++)
            {


                TMP_InputField newInput = Instantiate(i_answer);
                newInput.transform.SetParent(i_answer.transform.parent.transform, false);
                newInput.transform.SetSiblingIndex(i);

            }
        }
       
        
        TMP_InputField[] inputLists = i_holder.GetComponentsInChildren<TMP_InputField>();
        bool check = true;
        var user_answers=questionResult.user_answer.Split(",");
        for (int i = 0; i < inputLists.Length; i++)
        {
            inputLists[i].interactable= false;
            inputLists[i].text = user_answers[i];
            if (inputLists[i].text != right_answers[i])
            {
                check = false;
                
            }
            

        }
        i_rightAnswer.text = "Đáp áp đúng là: "+ exercise.right_answer;
        
        if (check)
        {
            i_result.sprite = correctSprite;
        }
        else
        {
            i_result.sprite = incorrectSprite;
        }
    }
}

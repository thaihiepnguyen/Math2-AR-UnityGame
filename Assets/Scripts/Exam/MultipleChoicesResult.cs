using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public partial class ReviewManager : MonoBehaviour
{
    [SerializeField]
    GameObject m_answerList;
    [SerializeField]
    TMP_Text m_question;
    [SerializeField]
    Image imageQuestion;
    
    private bool isImageQuestion = false;
    
    void ChangeMultipleChoiceObject(ExerciseDTO exercise,QuestionResultDTO questionResult)
    {
        var questions = exercise.question;
        var answers = exercise.answer.Split(",");

        m_question.text = questions;

        if (exercise.image_url != null)
        {
            isImageQuestion = true;
            StartCoroutine(LoadImageManager.LoadImage(imageQuestion, exercise.image_url));
        
            
            imageQuestion.gameObject.SetActive(true);
        }
        else
        {
            
            imageQuestion.gameObject.SetActive(false);
        }
        Debug.Log("TransformAnswer " + m_answerList.transform.position.ToString());
        Button[] buttons = m_answerList.GetComponentsInChildren<Button>();

        for (int i = 0; i < buttons.Length; i++)
        {
            TextMeshProUGUI tmp = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null)
            {
                tmp.text = answers[i];
            }
            else
            {
                Debug.LogError("TextMeshPro component not found in children of button.");
            }
        }
        var rightAnswer = exercises[currentQuestion].right_answer;

        Image[] answerObjects = m_answerList.GetComponentsInChildren<Image>();

        for (int i = 0; i < answerObjects.Length; i++)
        {
            TextMeshProUGUI tmp = answerObjects[i].GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null)
            {
                if (tmp.text == rightAnswer)
                {
                    answerObjects[i].color = Color.green;
                }
                else if (tmp.text == questionResult.user_answer)
                {
                    answerObjects[i].color = HexToColor("#FFB45D");
                }
                else
                {
                    answerObjects[i].color = Color.red;
                }
            }
            else
            {
                Debug.LogError("TextMeshPro component not found in children of button.");
            }
        }
        
    }
    private Color HexToColor(string hex)
    {
        Color color = Color.white;
        if (UnityEngine.ColorUtility.TryParseHtmlString(hex, out color))
        {
            return color;
        }
        else
        {
            Debug.LogError("Invalid hexadecimal color: " + hex);
            return Color.white;
        }
    }
   
}

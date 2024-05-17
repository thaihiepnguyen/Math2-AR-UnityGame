using System.Collections;
using EasyUI.Toast;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public partial class ExerciseMananger : MonoBehaviour
{
    [SerializeField]
    Canvas MultipleChoiceExercise;
    [SerializeField]
    GameObject m_answerList;
    [SerializeField]
    TMP_Text m_question;
    [SerializeField]
    Image imageQuestion;
    private bool isImageQuestion = false;

    void ChangeMultipleChoiceObject(ExerciseDTO exercise)
    {
        var questions = exercise.question;
        var answers = exercise.answer.Split(",");

        if (exercise.image_id != null)
        {
            isImageQuestion = true;
            StartCoroutine(LoadImageManager.LoadBinaryImage(imageQuestion, (int)exercise.image_id));
            Vector3 currentPosition = m_answerList.transform.position;
            currentPosition.x += 500f;
            m_answerList.transform.position = currentPosition;
            imageQuestion.gameObject.SetActive(true);
        }

        m_question.text = questions;

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

        

    }

       bool checkAnswer = false;
    public void markAnswer(Image btn)
    {
        ResetMultipleChoiceAnswerAttribute();
        btn.color = HexToColor("#FFB45D");
        checkAnswer = true;
    }

    private void checkAnswerOfAChoice(Image btn, string rightAnswer)
    {
        TextMeshProUGUI answer = btn.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        if (answer != null)
        {
            if (answer.text == rightAnswer)
            {
                if (btn.color != HexToColor("#FFFFFF"))
                {
                    currentRightAnswer += 1;
                }
                btn.color = HexToColor("#00FF1E");
            }
            else if (btn.color != HexToColor("#FFFFFF"))
            {
                btn.color = HexToColor("#FF0000");
            }
        }

        checkAnswer = false;
    }

    private void ResetMultipleChoiceAnswerAttribute()
    {
        Image[] answerObjects = m_answerList.GetComponentsInChildren<Image>();

        for (int i = 0; i < answerObjects.Length; i++)
        {
            answerObjects[i].color = HexToColor("#FFFFFF");
        }
    }

    public void CheckMultipleChoiceAnswer()
    {
        if (!blockIO)
        {
            var rightAnswer = exercises[currentQuestion].right_answer;

            Image[] answerObjects = m_answerList.GetComponentsInChildren<Image>( );

         
            if (checkAnswer){
            for (int i = 0; i < answerObjects.Length; i++)
            {
                checkAnswerOfAChoice(answerObjects[i], rightAnswer);
            }
            AddExerciseToReviewList();
            if (isImageQuestion == true)
            {
                isImageQuestion = false;
                imageQuestion.gameObject.SetActive(false);
                // Vector3 currentPosition = m_answerList.transform.position;
                // currentPosition.x -= 500f;
                // m_answerList.transform.position = currentPosition;
            }
            StartCoroutine(NextQuestion());
            }
            else {
                  Toast.Show("Bạn hãy hoàn thành câu hỏi của mình nhé", .7f,ToastPosition.MiddleCenter);
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

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class ExerciseMananger : MonoBehaviour
{
    [SerializeField]
    Canvas MultipleChoiceExercise;
    [SerializeField]
    GameObject m_answerList;
    [SerializeField]
    TMP_Text m_question;


    void ChangeMultipleChoiceObject(ExerciseDTO exercise)
    {
        var questions = exercise.question;
        var answers = exercise.answer.Split(",");

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

    private void checkAnswerOfAChoice(Image btn, string rightAnswer)
    {
        TextMeshProUGUI answer = btn.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        if (answer != null)
        {
            if (answer.text == rightAnswer)
            {
                btn.color = HexToColor("#00FF1E");
                currentRightAnswer += 1;
            }
            else
            {
                btn.color = HexToColor("#FF0000");
            }
        }
    }

    private void ResetMultipleChoiceAnswerListColor()
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

            Image[] answerObjects = m_answerList.GetComponentsInChildren<Image>();

            for (int i = 0; i < answerObjects.Length; i++)
            {
                checkAnswerOfAChoice(answerObjects[i], rightAnswer);
            }
            AddExerciseToReviewList();
            StartCoroutine(NextQuestion());
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

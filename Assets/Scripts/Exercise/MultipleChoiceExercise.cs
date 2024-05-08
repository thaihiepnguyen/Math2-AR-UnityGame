using System.Collections;
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

        Debug.Log(exercise.image_url);
        if (exercise.image_url != null)
        {
            Debug.Log(exercise.image_url);
            isImageQuestion = true;
            StartCoroutine(LoadImage(imageQuestion, exercise.image_url));
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

    public void markAnswer(Image btn)
    {
        ResetMultipleChoiceAnswerAttribute();
        btn.color = HexToColor("#FFB45D");
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

            for (int i = 0; i < answerObjects.Length; i++)
            {
                checkAnswerOfAChoice(answerObjects[i], rightAnswer);
            }
            AddExerciseToReviewList();
            if (isImageQuestion == true)
            {
                isImageQuestion = false;
                imageQuestion.gameObject.SetActive(false);
                Vector3 currentPosition = m_answerList.transform.position;
                currentPosition.x -= 500f;
                m_answerList.transform.position = currentPosition;
            }
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

    private IEnumerator LoadImage(Image image, string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite newSprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f));
            image.sprite = newSprite;
        }
    }
}

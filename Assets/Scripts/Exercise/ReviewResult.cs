using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class ExerciseMananger : MonoBehaviour
{
    [SerializeField]
    Canvas ReviewResult;
    [SerializeField]
    Canvas ReviewList;
    [SerializeField]
    Button NextBtn;
    [SerializeField]
    Button PrevBtn;
    [SerializeField]
    TMP_Text result;
    private int currentResult = 0;
    private List<GameObject> reviewList = new List<GameObject>();

    private void AddExerciseToReviewList()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Exercise");
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].activeSelf)
            {
                GameObject reviewObj = Instantiate(objs[i]);
                reviewObj.SetActive(false);
                reviewObj.transform.SetParent(ReviewList.transform);
                reviewList.Add(reviewObj);
            }
        }
    }

    public void OnClickReview()
    {
        ReviewResult.gameObject.SetActive(false);
        ReviewList.gameObject.SetActive(true);
        NextBtn.gameObject.SetActive(true);
        PrevBtn.gameObject.SetActive(true);
        reviewList[0].gameObject.SetActive(true);
    }

    public void OnClickNextButton()
    {
        if (currentResult < reviewList.Count - 1)
        {
            reviewList[currentResult].gameObject.SetActive(false);
            currentResult++;
            reviewList[currentResult].gameObject.SetActive(true);
        }
    }

    public void OnClickPrevButton()
    {
        if (currentResult > 0)
        {
            reviewList[currentResult].gameObject.SetActive(false);
            currentResult--;
            reviewList[currentResult].gameObject.SetActive(true);
        }
    }
}
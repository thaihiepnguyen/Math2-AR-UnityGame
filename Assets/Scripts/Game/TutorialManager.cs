using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialManager : MonoBehaviour
{
    [SerializeField] Sprite[] tutorialSprites;
    [SerializeField] Button nextButton;
    [SerializeField] Button prevButton;
    [SerializeField] Image tutorialImage;

    private int currentImage;
    // Start is called before the first frame update
    void Start()
    {
        currentImage = 0;
        tutorialImage.sprite = tutorialSprites[0];
        prevButton.interactable = false;
        nextButton.interactable = true;
        nextButton.onClick.AddListener(ShowNextTutorial);
        prevButton.onClick.AddListener(ShowPrevTutorial);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowNextTutorial()
    {
        if (currentImage < tutorialSprites.Length - 1)
        {
            tutorialImage.sprite = tutorialSprites[++currentImage];
            if (currentImage  >= tutorialSprites.Length - 1)
            {
                nextButton.interactable = false;
            }
            prevButton.interactable = true;
        }
    }

    public void ShowPrevTutorial()
    {
        if (currentImage > 0)
        {
            tutorialImage.sprite = tutorialSprites[--currentImage];
            if (currentImage  <= 0)
            {
                prevButton.interactable = false;
            }
            nextButton.interactable = true;
        }
    }

    private void OnDisable()
    {
        currentImage = 0;
        tutorialImage.sprite = tutorialSprites[0];
        prevButton.interactable = false;
        nextButton.interactable = true;
    }
}

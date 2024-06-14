using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameCompleteManager : MonoBehaviour
{
    [SerializeField] GameObject lightRay;

    [SerializeField] GameObject bannerRibbon;

   

    [SerializeField] GameObject homeButton;

    [SerializeField] GameObject replayButton;

    [SerializeField] GameObject reward;

    [SerializeField] GameObject score;

    [SerializeField] GameObject star1;

    [SerializeField] GameObject star2;

    [SerializeField] GameObject star3;

    // void Awake(){

    //     LeanTween.reset();
    // }

    // void Start()
    // {
    //     LeanTween.rotateAround(lightRay, Vector3.forward, -360, 10f).setLoopClamp();
    //     LeanTween.scale(bannerRibbon, new Vector3(1.15f, 1.15f, 1.15f), 1.5f).setDelay(.5f).setEase(LeanTweenType.easeOutElastic).setOnComplete(LevelComplete);
    //     // LeanTween.moveLocal(bannerRibbon, new Vector3(-30f, 747f, 2f), .7f).setDelay(2f).setEase(LeanTweenType.easeInOutCubic);
    //     LeanTween.scale(bannerRibbon, new Vector3(1f,1f,1f),2f).setDelay(1.7f).setEase(LeanTweenType.easeInOutCubic);
    
    // }

    // void LevelComplete(){
    //     // LeanTween.moveLocal(backPanel, new Vector3(0f,-267f,0f),0.7f).setDelay(.5f).setEase(LeanTweenType.easeOutCirc);
    //     LeanTween.scale(homeButton, new Vector3(1f,1f,1f),2f).setDelay(.8f).setEase(LeanTweenType.easeOutElastic);
    //     LeanTween.scale(replayButton, new Vector3(1f,1f,1f),2f).setDelay(.9f).setEase(LeanTweenType.easeOutElastic);
    //     LeanTween.alpha(reward.GetComponent<RectTransform>(),1f,.5f).setDelay(1f);
    //     LeanTween.alpha(score.GetComponent<RectTransform>(),1f,.5f).setDelay(1f).setOnComplete(starAnim);
    // }

    // void starAnim(){
    //      LeanTween.scale(star1, new Vector3(1f,1f,1f),2f).setEase(LeanTweenType.easeOutElastic);
    //     LeanTween.scale(star2, new Vector3(1f,1f,1f),2f).setDelay(.1f).setEase(LeanTweenType.easeOutElastic);
    //      LeanTween.scale(star3, new Vector3(1f,1f,1f),2f).setDelay(.2f).setEase(LeanTweenType.easeOutElastic);
    // }

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI maxScoreText;

    [SerializeField] private TextMeshProUGUI titleText;


    void Start(){
    //    int sc = GameObject.FindObjectOfType<NumberRunnerManager>().GetScore;
    //    scoreText.text = string.Format("Tổng điểm: {0}", sc.ToString());
    //    int msc = NumberRunnerManager.GetMaxScore();
    //     maxScoreText.text = string.Format("Tổng điểm: {0}", sc.ToString());

    //     starAchieved();
    }

    public void OnComplete(string score, string maxScore){
               scoreText.text = string.Format("Tổng điểm: {0}", score);
               maxScoreText.text = string.Format("Điểm cao nhất: {0}", maxScore);



    }
    public void starAchieved(int currentHealth, int maxHealth){
      
      
        float percentage = (currentHealth/ (float) maxHealth) * 100;

        if (percentage < 50){
            star1.SetActive(true);
            titleText.text= "Khá";
        }
        else if (percentage >=50 && percentage < 80){
            star1.SetActive(true);
            star2.SetActive(true);
               titleText.text= "Giỏi";
        }
        else if (percentage >= 80){
            star1.SetActive(true);
            star2.SetActive(true);
            star3.SetActive(true);
            titleText.text= "Xuất sắc";
        }


    }

}

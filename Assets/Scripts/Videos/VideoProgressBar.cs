using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoProgressBar : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] VideoPlayer player;
    Slider progressSlider;
    bool isDragging = false;
    public Sprite pauseSprite;
    public Sprite playSprite;
    public Image img;
    public Button middlePausePlayBtn;
    void Start()
    {
        
        progressSlider = GetComponent<Slider>();
        Debug.Log("Time " + player.time);
        progressSlider.onValueChanged.AddListener(HandleProgressBarValueChanged);
    }

    public void HandleProgressBarValueChanged(float value)
    {
        if (player && isDragging)
        {

            player.time = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isPlaying && !isDragging && player)
        {
            progressSlider.value = (float)player.time;
            if (progressSlider.maxValue != (float)player.length)
            {
                progressSlider.maxValue = (float)player.length;
            }
        }
    }

    public void StartDrag()
    {
        isDragging = true;
        player.Pause();
    }

    public void EndDrag()
    {
        player.Play();
        isDragging = false;
    }

    public void PauseVideo()
    {
        middlePausePlayBtn.gameObject.SetActive(true);
        img.sprite = playSprite;
        player.Pause();
    }

    public void PlayVideo()
    {
        middlePausePlayBtn.gameObject.SetActive(false);
        img.sprite = pauseSprite;
        player.Play();
    }

    public void handleProgressBarBtnClicked()
    {
        if (player.isPlaying && player)
        {
            PauseVideo();
        }
        else
        {
            PlayVideo();
        }
    }
    public void Exit()
    {
        SceneManager.LoadScene("VideoLearning");
    }
}

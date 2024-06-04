using EasyUI.Toast;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArrowGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject hearts;
    [SerializeField] TextMeshProUGUI question;
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] TextMeshProUGUI progress;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;
    [SerializeField] GameObject gameOverMenu;
    public int totalquestions = 10;
    public int curquestion = 0;
    private int maxhealth = 3;
    public  int curhealth { get;  set; }
    private static ArrowGameManager instance;
    public  static ArrowGameManager GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        curhealth = maxhealth;                            
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(curhealth <= 0)
        {
            Toast.Show("Game Over");
            gameOverMenu.gameObject.SetActive(true);
        }
    }
    public void DescreaseHealth()
    {
        curhealth -= 1;
        curhealth = Mathf.Clamp(curhealth, 0, maxhealth);
        if (curhealth < maxhealth)
        {
            var heart=hearts.GetComponentsInChildren<Image>();
            heart[curhealth].sprite = emptyHeart;
        }
        Debug.Log("Health "+ curhealth);
    }
    public void IncreaseHeakth()
    {
        curhealth += 1;
        curhealth = Mathf.Clamp(curhealth, 0, maxhealth);
    }
    public void OnRestart()
    {
        SceneManager.LoadScene("ArrowGame");
    }
    public void OnExit()
    {
        SceneHistory.GetInstance().PreviousScene();
    }
}

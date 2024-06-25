using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using EasyUI.Toast;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CatGameManager : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private List<GameObject> spawnObjects = new List<GameObject>();
    [SerializeField] private float scaler;
    [SerializeField] private Camera mainCam;
    [SerializeField] GameObject MainUI;
    [SerializeField] GameObject WaitingUI;
    [SerializeField] GameObject NextQuestionUI;
    public int curquestion = 0;
    public int totalquestions = 10;
    public int point = 0;
    private int numberOfRightAns = 0;
    public int amount;
    public int currentAmount;
    bool isGameOver = false;
    bool isEggsCreated = false;

    [Header("UI")]
    [SerializeField] GameObject hearts;
    [SerializeField] TextMeshProUGUI question;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;    
    
    [SerializeField] TextMeshProUGUI progress;
    // [SerializeField] GameObject targets;
    [SerializeField] TextMeshProUGUI yourScore;
   

    [Header("Audio")]
    [SerializeField] AudioClip correctSFX;
    [SerializeField] AudioClip wrongSFX;
    [SerializeField] AudioClip winSFX;
    [SerializeField] AudioClip gameoverSFX;
    private GameObject spawnedObject;
    private int maxhealth = 5;
    public int curhealth { get; set; }
    private AudioSource audioSource;
    private GameObject trackables;
    private List<ExerciseDTO> exerciseList = new List<ExerciseDTO>();
    private static CatGameManager instance;
    

    public static CatGameManager GetInstance()
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
    GameObject GetRandomObject()
    {
        return spawnObjects[Random.Range(0, spawnObjects.Count)];
    }
    void InstantiateObject(GameObject obj)
    {
        spawnedObject = Instantiate(obj, GetRandomVector(), Quaternion.identity);
        spawnedObject.transform.localScale *= scaler;
    }
    Vector3 GetRandomVector()
    {
        GameObject dragon = GameObject.Find("Blue");
        if(dragon != null){
            Vector3 randomVector = new Vector3(
            dragon.transform.position.x + Random.Range(-2f, 2f), 
            dragon.transform.position.y + Random.Range(-2f, -0.5f), 
            dragon.transform.position.z + Random.Range(0.5f, 2f)
            );
            while(IsCreated(randomVector))
                randomVector = new Vector3(
                    dragon.transform.position.x + Random.Range(-2f, 2f), 
                    dragon.transform.position.y + Random.Range(-2f, -0.5f), 
                    dragon.transform.position.z + Random.Range(-2f, 2f)
                );
            return randomVector;
        }
        else{
        Vector3 randomVector = new Vector3(
            mainCam.transform.position.x + Random.Range(-2f, 2f), 
            mainCam.transform.position.y + Random.Range(-2f, -0.5f), 
            mainCam.transform.position.z + Random.Range(0.5f, 2f)
        );
        while(IsCreated(randomVector))
            randomVector = new Vector3(
                mainCam.transform.position.x + Random.Range(-2f, 2f), 
                mainCam.transform.position.y + Random.Range(-2f, -0.5f), 
                mainCam.transform.position.z + Random.Range(-2f, 2f)
            );
        return randomVector;
        }
    }
    private bool IsCreated(Vector3 position){
        float raycastDistance = 0.1f;
        if (Physics.Raycast(position, Vector3.forward, raycastDistance))
            return true;
        return false;
    }
    // Start is called before the first frame update
    void Start()
    {
        ExerciseDTO exerciseDTO = new ExerciseDTO
        {
            question = "10 + 20 =",
            answer = "10,20,30,40",
            right_answer = "30"
        };
        ExerciseDTO exerciseDTO1 = new ExerciseDTO
        {
            question = "20 + 20 =",
            answer = "20,30,40,50",
            right_answer = "40"
        };
        exerciseList.Add(exerciseDTO);
        exerciseList.Add(exerciseDTO1);
        curhealth = maxhealth;
        updateUI();
        currentAmount = 0;
        audioSource = GetComponent<AudioSource>(); 
    }
    // Update is called once per frame
    void Update()
    {
        if(!isEggsCreated && currentAmount == amount){
            var answers = exerciseList[curquestion].answer.Split(',');
            for(int i = 0; i < amount; i++){
                var GameObject = GetRandomObject();
                GameObject.GetComponentInChildren<TextMeshProUGUI>().text = answers[Random.Range(0,answers.Length)];
                if(GameObject.GetComponentInChildren<TextMeshProUGUI>().text == exerciseList[curquestion].right_answer) numberOfRightAns++;
                InstantiateObject(GameObject);
            }
            isEggsCreated = true;
        }
    }
    void FixedUpdate(){
        if (curhealth <= 0 && !isGameOver)
        {
            Toast.Show("Game Over", 3f);
            OnGameEnd();
            isGameOver = true;
        }
        if (currentAmount < amount)
        {
            MainUI.SetActive(false);
            WaitingUI.SetActive(true);
        }
        else
        {
            WaitingUI.SetActive(false);
            MainUI.SetActive(true);  
        }
    }
    public void DecreaseHealth()
    {
        curhealth -= 1;
        curhealth = Mathf.Clamp(curhealth, 0, maxhealth);
        StartCoroutine(PlaySoundAfterSeconds(wrongSFX, 0.5f));
        UpdateHeartsUI();
        Debug.Log("Health " + curhealth);
    }
    public void IncreaseHealth()
    {
        curhealth += 1;
        curhealth = Mathf.Clamp(curhealth, curhealth, maxhealth);
        StartCoroutine(PlaySoundAfterSeconds(correctSFX, 0.5f));
        UpdateHeartsUI();
    }
    private void UpdateHeartsUI()
    {
        var heartImages = hearts.GetComponentsInChildren<Image>();
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].sprite = i < curhealth ? fullHeart : emptyHeart;
        }
    }
    IEnumerator PlaySoundAfterSeconds(AudioClip audioClip, float second)
    {
        yield return new WaitForSeconds(second);
        audioSource.clip = audioClip;
        audioSource.Play();
    }
    public void NextQuestion()
    {
        Thread.Sleep(500);
        MainUI.SetActive(false);
        NextQuestionUI.SetActive(true);
        curquestion++;
        if(curquestion  == totalquestions)
        {
            OnGameEnd();
            return;
        }
        var activeTarget = GameObject.FindGameObjectsWithTag("Spawnable");
        foreach(GameObject t in activeTarget)
        {
            Destroy(t);
        }
        isEggsCreated = false;
        // var curMeshes = trackables.GetComponentsInChildren<MeshFilter>();
        // foreach(var m in curMeshes)
        // {
        //     Destroy(m.gameObject);
        // }
        // amount = 10;
        updateUI();
    }
    void updateUI()
    {
        totalquestions = exerciseList.Count;
        progress.text = $"Câu {curquestion + 1}/{totalquestions}";
        question.text = exerciseList[curquestion].question;

        // var target_text = targets.GetComponentsInChildren<TextMeshProUGUI>();
        var answers = exerciseList[curquestion].answer.Split(',');

        // if (answers.Length != target_text.Length)
        // {
        //     //Debug.LogError("Số lượng đáp án không khớp với số lượng mục tiêu!");
        //     return;
        // }

        // for (int i = 0; i < answers.Length; i++)
        // {
        //     target_text[i].text = answers[i];
        // }
    }
    public void CheckAnswer(string answer)
    {
        if (exerciseList[curquestion].right_answer==answer)
        {
            point++;
            IncreaseHealth();
            if(point == numberOfRightAns){
                Debug.Log("get here");
                NextQuestion();
            }   
        }
        else
        {
            DecreaseHealth();
        }
    }
    public void OnGameEnd()
    {
        numberOfRightAns = 0;
        yourScore.text = (point*100).ToString();
       
        if (point > 0)
        {
            audioSource.clip = winSFX;
            audioSource.Play();
            
        }
        else
        {
            audioSource.clip = gameoverSFX;
            audioSource.Play();
        }
        
    }
    public void OnRestart()
    {
        SceneManager.LoadScene("CatGame");
    }
    public void OnExit()
    {
        //SceneHistory.GetInstance().PreviousScene();
    }
    public void OnContinue()
    {
        NextQuestionUI.SetActive(false);
        MainUI.SetActive(true);
    }
}

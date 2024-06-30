using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BubbleGameManager : MonoBehaviour
{
    public GameObject[] prefabs; // Array to hold the prefabs to instantiate
    public float spawnInterval = 2.0f; // Interval in seconds between spawns
    public Vector3 spawnAreaMin; // Minimum bounds of the spawn area
    public Vector3 spawnAreaMax; // Maximum bounds of the spawn area

    private Timer timer;
    private bool isGameOver;
    private bool isNearlyOverTime;

    [SerializeField] GameObject gameCompleteScreen;
    [SerializeField] GameObject hpBar;
    [SerializeField] GameObject bubbleGameTutorial;

    [SerializeField] AudioClip endSound;
    [SerializeField] AudioClip correctSound;
    [SerializeField] AudioClip incorrectSound;
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip hurryUpSound;

    [SerializeField] TextMeshProUGUI currentPointText;
    [SerializeField] TextMeshProUGUI playerPointText;
    [SerializeField] TextMeshProUGUI highestPointText;
    [SerializeField] TextMeshProUGUI rewardText;
    [SerializeField] TextMeshProUGUI progress;
    [SerializeField] TextMeshProUGUI questionText;

    [SerializeField] Sprite emptyHeart;

    private AudioSource audioSource;
    private int MAX_NUM_BUBBLE = 30;
    private int currentQuestion;
    private int currentHp;
    private int currentPoint;
    private int highestPoint = 10;

    private int questionNum = 10;

    void Start()
    {
        InvokeRepeating("SpawnPrefabs", 2.0f, spawnInterval);
        timer = GetComponent<Timer>();
        isGameOver = false;
        isNearlyOverTime = false;
        gameCompleteScreen.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        currentHp = 3;
        currentPoint = 0;
        currentPointText.text = "Điểm: " + currentPoint.ToString();
        GetQuestion();
    }

    void Update()
    {
        // Check touch and handle interact
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.CompareTag("Bubble"))
                    {
                        InteractWithObject(hit.collider.gameObject);
                    }
                }
            }
        }
        // Check time and play sound hurry up
        if (timer.timeValue <= 10 && isNearlyOverTime == false)
        {
            isNearlyOverTime = true;
            audioSource.clip = hurryUpSound;
            audioSource.Play();
        }
        if ((timer.timeValue <= 0 || currentHp == 0 || currentPoint == questionNum) && isGameOver == false)
        {
            isGameOver = true;
            gameCompleteScreen.SetActive(true);
            playerPointText.text = "Điểm của bạn: " + currentPoint.ToString();
            highestPointText.text = "Điểm cao nhất: " + highestPoint.ToString();
            rewardText.text = (currentPoint * 10).ToString();
            if (currentPoint == questionNum)
            {
                audioSource.clip = winSound;
                audioSource.Play();
            }
            else
            {
                audioSource.clip = endSound;
                audioSource.Play();
            }
        }
    }

    void SpawnPrefabs()
    {
        int numPrefabsToSpawn = Random.Range(2, 6);
        for (int i = 0; i < numPrefabsToSpawn; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y),
                Random.Range(spawnAreaMin.z, spawnAreaMax.z)
            );

            int randomPrefabIndex = Random.Range(0, prefabs.Length);
            var bubble = Instantiate(prefabs[randomPrefabIndex], randomPosition, Quaternion.identity);
            bubble.GetComponent<Renderer>().material.SetColor("_TintColor", HexToColor(GenerateRandomColorCode()));
            
            var bubbleContainer = GameObject.FindGameObjectWithTag("BubbleContainer");
            bubble.transform.SetParent(bubbleContainer.transform);

            var answer = Random.Range(currentQuestion - 5, currentQuestion + 5);
            bubble.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = answer.ToString();

            if (bubbleContainer.transform.childCount > MAX_NUM_BUBBLE)
            {
                Destroy(bubbleContainer.transform.GetChild(0).gameObject);
            }
        }
    }

    void InteractWithObject(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            var answer = obj.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text;
            if (answer == currentQuestion.ToString())
            {
                currentPoint++;
                audioSource.clip = correctSound;
                audioSource.Play();
                
                currentPointText.text = "Điểm: " + currentPoint.ToString();
                renderer.transform.GetChild(2).gameObject.SetActive(true);
                Destroy(obj, 1f);

                GetQuestion();
            }
            else
            {
                audioSource.clip = incorrectSound;
                audioSource.Play();
                renderer.material.SetColor("_TintColor", HexToColor("#FF6161"));
                hpBar.transform.GetChild(3 - currentHp).gameObject.GetComponent<Image>().sprite = emptyHeart;
                currentHp--;

            }
        }
    }

    private Color HexToColor(string hex)
    {
        Color color = Color.white;
        if (ColorUtility.TryParseHtmlString(hex, out color))
        {
            return color;
        }
        else
        {
            Debug.LogError("Invalid hexadecimal color: " + hex);
            return Color.white;
        }
    }

    void GetQuestion()
    {
        if (currentPoint < questionNum)
        {
            progress.text = (currentPoint + 1).ToString() + "/" + questionNum.ToString();
            currentQuestion = Random.Range(0, 30);
            questionText.text = "Chọn số sau: " + currentQuestion.ToString();
        }
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Exit()
    {
        SceneHistory.GetInstance().PreviousScene();
    }
    public void ShowTutorial()
    {
        bubbleGameTutorial.SetActive(true);
    }
    public void HideTutorial()
    {
        bubbleGameTutorial.SetActive(false);
    }
    private string GenerateRandomColorCode()
    {
        int randomValue = Random.Range(0, 0xFFFFFF + 1);
        return "#" + randomValue.ToString("X6");
    }
}   

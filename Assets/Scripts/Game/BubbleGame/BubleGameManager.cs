using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BubbleGameManager : MonoBehaviour
{
    public GameObject[] prefabs; // Array to hold the prefabs to instantiate
    public float spawnInterval = 2.0f; // Interval in seconds between spawns
    public Vector3 spawnAreaMin; // Minimum bounds of the spawn area
    public Vector3 spawnAreaMax; // Maximum bounds of the spawn area

    private Timer timer;
    private bool isGameOver;

    [SerializeField] GameObject resultModal;
    [SerializeField] AudioClip endSound;
    [SerializeField] GameObject question;

    private AudioSource audioSource;

    private int MAX_NUM_BUBBLE = 30;

    private int currentQuestion;

    private int hp;

    void Start()
    {
        InvokeRepeating("SpawnPrefabs", 2.0f, spawnInterval);
        timer = GetComponent<Timer>();
        isGameOver = false;
        resultModal.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        hp = 3;

        GetQuestion();
    }

    void Update()
    {
        // Check if there are any touches
        if (Input.touchCount > 0)
        {
            // Get the first touch
            Touch touch = Input.GetTouch(0);

            // Check if the touch began
            if (touch.phase == TouchPhase.Began)
            {
                // Convert touch position to a Ray
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                // Perform a raycast
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    // Check if the hit object has a specific tag or component
                    if (hit.collider.CompareTag("Bubble"))
                    {
                        // Handle interaction with the object
                        InteractWithObject(hit.collider.gameObject);
                    }
                }
            }
        }
        if (timer.timeValue <= 0 || hp == 0)
        {
            if (isGameOver == false)
            {
                isGameOver = true;
                resultModal.SetActive(true);
                audioSource.clip = endSound;
                audioSource.Play();
            
            }
        }
    }

    void SpawnPrefabs()
    {
        int numPrefabsToSpawn = Random.Range(2, 4); 
        for (int i = 0; i < numPrefabsToSpawn; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y),
                Random.Range(spawnAreaMin.z, spawnAreaMax.z)
            );

            int randomPrefabIndex = Random.Range(0, prefabs.Length);
            var bubble = Instantiate(prefabs[randomPrefabIndex], randomPosition, Quaternion.identity);
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
                Destroy(obj);
                GetQuestion();
            }
            else
            {
                renderer.material.color = Color.red;
                hp--;
            }
        }
    }

    void GetQuestion()
    {
        currentQuestion = Random.Range(0, 30);
        question.GetComponent<TextMeshProUGUI>().text = "Chọn số sau: " + currentQuestion.ToString();
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Exit()
    {
        SceneHistory.GetInstance().PreviousScene();
    }
}   

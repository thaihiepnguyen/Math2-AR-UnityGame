using EasyUI.Toast;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ArrowGameManager : MonoBehaviour
{
    [Header("Main UI")]
    [SerializeField] GameObject hearts;
    [SerializeField] GameObject targets;
    [SerializeField] GameObject targetPrefab;
    [SerializeField] TextMeshProUGUI question;    
    [SerializeField] TextMeshProUGUI progress;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;
    [SerializeField] GameObject MainUI;
    [SerializeField] GameObject WaitingUI;
    //Win menu
    [Header("Win UI")]
    [SerializeField] GameObject gameWinMenu;
    
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI yourScore;
    [SerializeField] TextMeshProUGUI yourHighestScore;
    [SerializeField] TextMeshProUGUI reward;
    public int point = 0;
    public int totalquestions = 10;
    public int curquestion = 0;
    private int maxhealth = 3;
    public int curhealth { get; set; }
    private const float minDistance = 2f;
    private static ArrowGameManager instance;
    private List<Vector3> spawnPositions = new List<Vector3>();
    [SerializeField] ARMeshManager meshManager;
    private int spawnCount = 4;
    public static ArrowGameManager GetInstance()
    {
        return instance;
    }

    public string GetCurRightAnswer()
    {
        return exerciseList[curquestion].right_answer;
    }

    private List<ExerciseDTO> exerciseList = new List<ExerciseDTO>();
    private GameObject trackables;
    private Mesh arMesh;
    private Timer timer;
    [Header("Audio")]
    [SerializeField] AudioClip correctSFX;
    [SerializeField] AudioClip wrongSFX;
    [SerializeField] GameObject overtimeSFX;
    [SerializeField] AudioClip winSFX;
    [SerializeField] AudioClip gameoverSFX;
    private AudioSource audioSource;
    bool isGameOver=false;
    bool isOverTime=false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

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
        trackables = GameObject.Find("Trackables");
        timer=GetComponent<Timer>();
        meshManager.meshesChanged += OnMeshesChanged;
        audioSource=GetComponent<AudioSource>(); 
    }

    private void OnMeshesChanged(ARMeshesChangedEventArgs obj)
    {
        SpawnTarget();
    }
    void SpawnTarget()
    {
        if (spawnCount <= 0)
        {
            return;
        }
        trackables = GameObject.Find("Trackables");

        // Lấy tất cả các lưới hiện tại
        IList<MeshFilter> meshes = meshManager.meshes;

        if (meshes.Count == 0) return;

        bool validPositionFound = false;
        int attempt = 0;
        const int maxAttempts = 100; // Giới hạn số lần thử

        while (!validPositionFound && attempt < maxAttempts)
        {
            attempt++;
            MeshFilter selectedMeshFilter = meshes[UnityEngine.Random.Range(0, meshes.Count)];
            if (selectedMeshFilter == null)
            {
                return;
            }
            Mesh selectedMesh = selectedMeshFilter.mesh;
            var meshAnalyser = selectedMeshFilter.gameObject.GetComponent<MeshAnalyserArrow>();
            // Find the highest vertex in the selected mesh
            if (!meshAnalyser.IsGround)
            {
                return;
            }
            Vector3 highestVertexPosition = selectedMesh.vertices[0];
            foreach (Vector3 vertex in selectedMesh.vertices)
            {
                if (vertex.y > highestVertexPosition.y)
                {
                    highestVertexPosition = vertex;
                }
            }
            // Chuyển đổi vị trí từ không gian local sang không gian world
            Vector3 worldPosition = selectedMeshFilter.transform.TransformPoint(highestVertexPosition);

            // Kiểm tra khoảng cách với các vị trí đã spawn
            bool isValid = true;
            foreach (Vector3 spawnPos in spawnPositions)
            {
                if (Vector3.Distance(worldPosition, spawnPos) < minDistance)
                {
                    isValid = false;
                    break;
                }
            }

            // Nếu vị trí hợp lệ, spawn đối tượng mục tiêu
            if (isValid)
            {
                spawnPositions.Add(worldPosition);
                var target = Instantiate(targetPrefab, worldPosition, Quaternion.identity, trackables.transform);
                var arCamera = Camera.main;
                if (arCamera != null)
                {
                    Vector3 directionToCamera = arCamera.transform.position - target.transform.position;
                    directionToCamera.y = 0; // Keep the target upright
                    Quaternion lookRotation = Quaternion.LookRotation(-directionToCamera);
                    target.transform.rotation = lookRotation;
                }
                target.transform.SetParent(targets.transform, true);
                validPositionFound = true;
                spawnCount--;
                if (spawnCount == 0)
                {
                    updateUI();
                }
            }
        }

        if (!validPositionFound)
        {
            Debug.LogWarning("Không thể tìm thấy vị trí hợp lệ để spawn đối tượng mục tiêu.");
        }

        
    }
    void updateUI()
    {
        totalquestions = exerciseList.Count;
        progress.text = $"Câu {curquestion + 1}/{totalquestions}";
        question.text = exerciseList[curquestion].question;

        var target_text = targets.GetComponentsInChildren<TextMeshProUGUI>();
        var answers = exerciseList[curquestion].answer.Split(',');

        if (answers.Length != target_text.Length)
        {
            //Debug.LogError("Số lượng đáp án không khớp với số lượng mục tiêu!");
            return;
        }

        for (int i = 0; i < answers.Length; i++)
        {
            target_text[i].text = answers[i];
        }
    }

    void FixedUpdate()
    {
        if (curhealth <= 0 && !isGameOver)
        {
            Toast.Show("Game Over", 3f);
            OnGameEnd();
            isGameOver = true;
        }
        //Ten seconds left
        if (timer.timeValue <=10  && !isOverTime)
        {
            overtimeSFX.GetComponent<AudioSource>().Play();
            isOverTime= true;
        }
        if (timer.timeValue <= 0 && !isGameOver)
        {
            isGameOver= true;
            OnGameEnd();
            
        }
        if (spawnCount > 0)
        {
            MainUI.SetActive(false);
            WaitingUI.SetActive(true);
            timer.isStop= true;
        }
        else
        {
            if(curhealth> 0)
            {
                WaitingUI.SetActive(false);
                MainUI.SetActive(true);
                timer.isStop = false; 
            }
           
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

    public void OnRestart()
    {
        SceneManager.LoadScene("ArrowGame");
    }

    public void OnExit()
    {
        //SceneHistory.GetInstance().PreviousScene();
    }
   public void NextQuestion()
    {
        curquestion++;
        if(curquestion  == totalquestions)
        {
            OnGameEnd();
            return;
        }
        var activeTarget = GameObject.FindGameObjectsWithTag("Target");
        foreach(GameObject t in activeTarget)
        {
            Destroy(t);
        }
        var curMeshes=trackables.GetComponentsInChildren<MeshFilter>();
        foreach(var m in curMeshes)
        {
            Destroy(m.gameObject);
        }
        spawnCount = 4;
        updateUI();
    }
    public void CheckAnswer(string answer)
    {
        if (exerciseList[curquestion].right_answer==answer)
        {
            point++;
            IncreaseHealth();
            NextQuestion();
        }
        else
        {
           DecreaseHealth();
        }
    }
    public void OnGameEnd()
    {
        timer.isStop = true;
        
        timeText.text = timer.toTimeString();
        yourScore.text = (point*100).ToString();
        yourHighestScore.text = yourScore.text;
       
        var realTimeValue = timer.baseTimeValue - timer.timeValue;
        if (point == 0) realTimeValue = 0;
        reward.text ="+ "+(point*10 + (int)Mathf.Round(Mathf.Clamp(realTimeValue, 0, realTimeValue))*10).ToString();
        gameWinMenu.gameObject.SetActive(true);
        overtimeSFX.GetComponent<AudioSource>().Stop();
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
    IEnumerator PlaySoundAfterSeconds(AudioClip audioClip, float second)
    {
        yield return new WaitForSeconds(second);
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}

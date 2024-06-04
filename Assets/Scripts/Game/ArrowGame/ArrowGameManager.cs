using EasyUI.Toast;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ArrowGameManager : MonoBehaviour
{
    [SerializeField] GameObject hearts;
    [SerializeField] GameObject targets;
    [SerializeField] GameObject targetPrefab;
    [SerializeField] TextMeshProUGUI question;
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] TextMeshProUGUI progress;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;
    [SerializeField] GameObject gameOverMenu;
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
        exerciseList.Add(exerciseDTO);
        curhealth = maxhealth;
        updateUI();
        trackables = GameObject.Find("Trackables");
        meshManager.meshesChanged += OnMeshesChanged;
    }

    private void OnMeshesChanged(ARMeshesChangedEventArgs obj)
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

            // Chọn ngẫu nhiên một lưới từ danh sách
            MeshFilter selectedMeshFilter = meshes[UnityEngine.Random.Range(0, meshes.Count)];
            Mesh selectedMesh = selectedMeshFilter.mesh;

            // Chọn ngẫu nhiên một đỉnh từ lưới
            int randomVertexIndex = UnityEngine.Random.Range(0, selectedMesh.vertexCount);
            Vector3 randomVertexPosition = selectedMesh.vertices[randomVertexIndex];

            // Chuyển đổi vị trí từ không gian local sang không gian world
            Vector3 worldPosition = selectedMeshFilter.transform.TransformPoint(randomVertexPosition);

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
                //target.transform.SetParent(targets.transform, true);
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

        Debug.Log(obj.ToString());
    }

    void updateUI()
    {
        totalquestions = exerciseList.Count;
        progress.text = $"Câu {curquestion + 1}/{totalquestions}";
        question.text = exerciseList[0].question;

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
        if (curhealth <= 0)
        {
            Toast.Show("Game Over", 3f);
            gameOverMenu.gameObject.SetActive(true);
        }
    }

    public void DecreaseHealth()
    {
        curhealth -= 1;
        curhealth = Mathf.Clamp(curhealth, 0, maxhealth);

        UpdateHeartsUI();
        Debug.Log("Health " + curhealth);
    }

    public void IncreaseHealth()
    {
        curhealth += 1;
        curhealth = Mathf.Clamp(curhealth, 0, maxhealth);
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
        SceneHistory.GetInstance().PreviousScene();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyUI.Toast;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BallGame : MonoBehaviour
{
    [SerializeField] Button throwBtn;
    [SerializeField] Slider slider;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] GameObject spawnParent;

    private GameObject currentBall;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;

    private AudioSource audioSource;

    [SerializeField] private AudioClip correctAudio;
    [SerializeField] private AudioClip incorrectAudio;
    [SerializeField] private AudioClip gameOverAudio;
    private Queue<GameObject> tempBalls = new Queue<GameObject>();
    private BallARPlane ballARPlaneInstance;

    private float startTime;

    private float MAX_DURATION = 2;
    private float MAX_MAGNITUDE = 50f;
    private float MAX_FORCE = 10f;
    public int REMAINING_HEART = 3;
    public int POINT = 0;
    public int currentExerciseIndex = 0;
    [SerializeField] private TextMeshProUGUI question;
    [SerializeField] private GameObject hearts;
    private Image[] heartImages;
    [SerializeField] private TextMeshProUGUI progress;

    public GameDTO gameDto = null;


    public void Awake() {
        gameDto = Lesson.GetGameDto();

        if (gameDto == null) {
            Toast.Show("Something Error!", 3f);
            return;
        }

        var gameData = gameDto.gameData;

        if (gameData.Length >= 1) {
            question.text = gameData[currentExerciseIndex].question;
            progress.text = "Câu " + (currentExerciseIndex + 1) + "/" + gameData.Length; 
        }
    }

    public void PlayCorrectAudio() {
        this.audioSource.clip = correctAudio;
        this.audioSource.Play();
    }

    public void PlayIncorrectAudio() {
        this.audioSource.clip = incorrectAudio;
        this.audioSource.Play();
    }

    public void PlayGameOverAudio() {
        this.audioSource.clip = gameOverAudio;
        this.audioSource.Play();
    }

    public void Start() {
        if (gameDto == null) {
            return;
        }
        currentBall = GenerateBall();
        slider.value = 0;
        var buttonHoldAndRelease = throwBtn.GetComponent<ButtonHoldAndRelease>();
        ballARPlaneInstance = GameObject.FindGameObjectWithTag("BallGameXROrigin").GetComponent<BallARPlane>();
        audioSource = gameObject.GetComponentInChildren<AudioSource>();
        heartImages = hearts.GetComponentsInChildren<Image>();
        buttonHoldAndRelease.OnButtonDownEvent += OnClickThrow;
        buttonHoldAndRelease.OnButtonHoldEvent += HoldThrow;
        buttonHoldAndRelease.OnButtonUpEvent += ReleaseThrow;
    }

    private GameObject GenerateBall() {
        var ball = Instantiate(ballPrefab);
        ball.transform.SetParent(spawnParent.transform);
        ball.transform.localPosition = new Vector3(0,0,0);
        var sphereCollider = ball.GetComponent<SphereCollider>();
        sphereCollider.enabled = false;

        return ball;
    }

    public void Update() {
        if (tempBalls.Count != 0) {
            var tempBall = tempBalls.Dequeue();

            if (tempBall.transform.position.magnitude > MAX_MAGNITUDE) {
                Destroy(tempBall);
            } else {
                tempBalls.Enqueue(tempBall);
            }
        }
    }

    private void OnClickThrow() {
        startTime = Time.time;
    }

    private void HoldThrow() {
        var duration = Time.time - startTime;

        slider.value = duration / MAX_DURATION;
        if (slider.value >= 1) {
            slider.value = 0;
            startTime = Time.time;
        }
    }

    private void ReleaseThrow() {
        var strenght = slider.value;

        Vector3 forward = Camera.main.transform.forward;
        Vector3 up = Camera.main.transform.up;
        Vector3 direction = (forward + up).normalized;

        var rigidbody = currentBall.GetComponent<Rigidbody>();
        var sphereCollider = currentBall.GetComponent<SphereCollider>();
        currentBall.transform.SetParent(null);
        rigidbody.useGravity = true;
        sphereCollider.enabled = true;

        rigidbody.AddForce(
            direction * strenght * MAX_FORCE, ForceMode.Impulse
        );

        tempBalls.Enqueue(currentBall);

        currentBall = GenerateBall();
        slider.value = 0;
    }

    public void UpdateUI() {
        if (REMAINING_HEART != heartImages.Length) {
            if (REMAINING_HEART < 0) {
                Toast.Show("Error!", 3f);
                return;
            }

            Image currentHeart = heartImages[REMAINING_HEART];
            currentHeart.sprite = emptyHeart;

            if (REMAINING_HEART == 0) {
                Toast.Show("Out of heart!", 3f);

                return;
            }
        }

        if (currentExerciseIndex < gameDto.gameData.Length) {
            question.text = gameDto.gameData[currentExerciseIndex].question;
            ballARPlaneInstance.UpdateRims(gameDto.gameData[currentExerciseIndex]);
            progress.text = "Câu " + (currentExerciseIndex + 1) + "/" + gameDto.gameData.Length; 
        }
        else {
            Toast.Show("Finished! : " + POINT, 3f);
        }
    }
}
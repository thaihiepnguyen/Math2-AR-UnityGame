using System.Collections;
using System.Collections.Generic;
using EasyUI.Toast;
using TMPro;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private BallGame ballGameInstance;

    // Start is called before the first frame update
    void Start()
    {
        ballGameInstance = GameObject.FindGameObjectWithTag("BallGameManager").GetComponent<BallGame>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("BallObject")) {
            var answer = gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
            var rightAnswer = ballGameInstance.gameDto.gameData[ballGameInstance.currentExerciseIndex].right_answer;

            if (answer == rightAnswer) {
                ballGameInstance.POINT++;
                ballGameInstance.PlayCorrectAudio();
            } else {
                ballGameInstance.REMAINING_HEART--;
                ballGameInstance.PlayIncorrectAudio();
            }

            ballGameInstance.currentExerciseIndex++;

            ballGameInstance.UpdateUI();
        }
    }
}

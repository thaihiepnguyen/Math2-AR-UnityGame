using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnTriggerHandler : MonoBehaviour
{
    [SerializeField] private TileQuizManager tile;
    [SerializeField] private TextMeshProUGUI answer;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player"){
            if (tile.CheckQuiz(answer.text)){
                answer.GetComponentInParent<Image>().color = new Color32(105,205,127,119);
            }
            else {
                answer.GetComponentInParent<Image>().color = new Color32(196,62,63,119);
            }
        }
    }
}

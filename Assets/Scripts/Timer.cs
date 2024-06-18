using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timer;
    [SerializeField] public float timeValue = 0.5f;
    public bool isStop = false;
    [SerializeField, HideInInspector]
    public float baseTimeValue { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        timeValue *= 60;
        baseTimeValue = timeValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStop)
        {
            if (timeValue > 0)
            {
                timeValue -= Time.deltaTime;
            }
            else
            {
                timeValue = 0;
            }
            DisplayTime(timeValue);
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }
        float minutes=Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public string toTimeString()
    {
        if (timeValue < 0)
        {
            timeValue = 0;
        }
        
        float realtime = baseTimeValue- timeValue;
        float minutes = Mathf.FloorToInt(realtime / 60);
        float seconds = Mathf.FloorToInt(realtime % 60);
        var res = string.Format("{0:00}:{1:00}", minutes, seconds);
        return res;
    }
}

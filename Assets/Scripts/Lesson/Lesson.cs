using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lesson : MonoBehaviour
{
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickDoingExercise()
    {
        SceneHistory.GetInstance().LoadScene(GlobalVariable.EXERCISES_SCENE);
    }
}

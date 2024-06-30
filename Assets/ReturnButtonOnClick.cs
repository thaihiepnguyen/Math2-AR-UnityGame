using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyUI.Progress;

public class ReturnButtonOnClick : MonoBehaviour
{
    public void OnClickButton()
    {
        Progress.Hide();
        SceneHistory.GetInstance().PreviousScene();
    }
}

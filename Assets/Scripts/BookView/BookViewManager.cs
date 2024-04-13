using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Rotate : MonoBehaviour
{
    public Button RotateButton;
    // Start is called before the first frame update
    void Start()
    {
        RotateButton.onClick.AddListener(OnClickRotateButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickRotateButton()
    {
        Debug.Log("Clicked!");
    }
}

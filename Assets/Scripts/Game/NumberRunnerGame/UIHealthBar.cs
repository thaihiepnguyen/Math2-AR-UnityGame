using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
     [SerializeField] private Slider slider;
    public void UpdateHealthBar(float currentValue, float maxValue){
        slider.value = currentValue / maxValue;
        if (slider.value < 0.5f){
            Color color = new Color(233f/255f, 79f/255f, 55f/255f);
            transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = color;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
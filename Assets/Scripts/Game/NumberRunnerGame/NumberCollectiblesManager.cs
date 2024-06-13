using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class NumberCollectiblesManager : MonoBehaviour
{

     [SerializeField]
    private TextMeshProUGUI OverlayText;
     
    private void OnTriggerEnter(Collider collision)
    {
       
        DragonController controller = collision.GetComponent<DragonController>();
        
        if (controller != null)
        {
             if (FindObjectOfType<NumberRunnerManager>().CheckCurrent( Int32.Parse(OverlayText.text), controller)){
            
               Destroy(gameObject);
             }
        } 
        
    }

   

  

    public void SetOverlayText(string text)
    {
        if(OverlayText != null)
        {
            OverlayText.text = text;
        }
    }

   
}

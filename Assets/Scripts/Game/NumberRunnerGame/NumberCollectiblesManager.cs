using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class NumberCollectiblesManager : MonoBehaviour
{

     [SerializeField]
    private TextMeshProUGUI OverlayText;

    [SerializeField]
    private TextMeshPro MinimapText;
     
    private void OnTriggerEnter(Collider collision)
    {
       
        DragonController controller = collision.GetComponent<DragonController>();
        
        if (controller != null)
        {
             if (FindObjectOfType<NumberRunnerManager>().CheckCurrent( int.Parse(OverlayText.text), controller)){
            
               Destroy(gameObject);
             }
        } 
        
    }

   
    private float randomY = 0f;
  
    void Awake()
    {
            randomY = Random.Range(10f, 20f);
    }

  
    public void SetText(string text)
    {
        if(OverlayText != null && MinimapText !=null)
        {
          
            OverlayText.text = text;
            MinimapText.SetText(text);

        Color randomColor = Color.white;
        while (randomColor == Color.white){
        float randomR = Random.Range(0f, 1f);
        float randomG = Random.Range(0f, 1f);
        float randomB = Random.Range(0f, 1f);

        randomColor = new Color (randomR, randomG, randomB);
        }
         

            MinimapText.color = randomColor;
        }
    }

   
}

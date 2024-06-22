using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class NumberCollectiblesManager : MonoBehaviour
{

     [SerializeField]
    private TextMeshProUGUI OverlayText;

    // [SerializeField]
    // private TextMeshProUGUI OverlayText;

      [SerializeField]
    private GameObject textHolder;
    

    [SerializeField]
    private TextMeshPro MinimapText;

    [SerializeField] private AudioClip collectedClip;

    [SerializeField] private ParticleSystem collectEffect;

    Transform mainCam;
    Transform unit;
    Transform worldSpaceCanvas;
     
     public Vector3 offset;

    private void OnTriggerEnter(Collider collision)
    {
       
        DragonController controller = collision.GetComponent<DragonController>();
        
        if (controller != null)
        {
             if (FindObjectOfType<NumberRunnerManager>().CheckCurrent( int.Parse(OverlayText.text), controller)){

                controller.PlaySound(collectedClip);
                
            
               Destroy(gameObject);
              Instantiate(collectEffect,transform.position,transform.rotation);
                    
    
             }
        } 
        
    }
        void Start()
    {
    //     mainCam = Camera.main.transform;
    //     unit = transform.parent;
            // Canvas world = OverlayText.GetComponentInParent<Canvas>();
            // world.worldCamera = Camera.main;

    //     OverlayText.transform.SetParent(worldSpaceCanvas);
     }
    void LateUpdate(){
        //   OverlayText.transform.rotation = Quaternion.LookRotation(Camera.main.transform.rotation + new Vector3(90,0,0));
        //  OverlayText.transform.position = unit.position + offset;

        // OverlayText.transform.rotation = Camera.main.transform.rotation +  new Vector3(90,0,0);

             OverlayText.transform.LookAt(Camera.main.transform);
          OverlayText.transform.RotateAround(OverlayText.transform.position, OverlayText.transform.up, 180f);
    
        // OverlayText.transform.rotation+= new Vector3(90f,0,0);

    }
   
    public bool checkText = false;
    public void SetText(string text)
    {
        if(OverlayText != null && MinimapText !=null)
        {
          
            // OverlayText.text = text;
            OverlayText.SetText(text);
            MinimapText.SetText(text);

        Color randomColor = Color.white;
        while (randomColor == Color.white){
        float randomR = Random.Range(0f, 1f);
        float randomG = Random.Range(0f, 1f);
        float randomB = Random.Range(0f, 1f);

        randomColor = new Color (randomR, randomG, randomB);
        }
         

            MinimapText.color = randomColor;
            OverlayText.color = randomColor;
            // OverlayText.transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position);
            // OverlayText.transform.LookAt(Camera.main.transform);

    
             checkText = true;
        }

       
    }

   
}

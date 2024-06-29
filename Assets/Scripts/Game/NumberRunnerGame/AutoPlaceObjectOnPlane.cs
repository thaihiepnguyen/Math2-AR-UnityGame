
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneManager))]

public class AutoPlaceOfObjectOnPlane : MonoBehaviour
{
    [SerializeField]
    private GameObject welcomePanel;

    [SerializeField]
    private GameObject placedPrefab;

    private GameObject placedObject;

    [SerializeField]
    private Button dismissButton;

    [SerializeField]
    private ARPlaneManager arPlaneManager;

    [SerializeField]
    private ARMeshManager arMeshManager;

    public bool doneSpawn = false;

    private Transform oTransform;
    GameObject spawnedObject;
 
 


    void Awake() 
    {
        dismissButton.onClick.AddListener(Dismiss);
        arPlaneManager = GetComponent<ARPlaneManager>();
        oTransform = transform;
   
        
        // arMeshManager.meshesChanged+= OnMeshChange;
        arPlaneManager.planesChanged += PlaneChanged;
        doneSpawn = false;

    }

    void OnMeshChange(ARMeshesChangedEventArgs args){
         if(args.added != null && placedObject == null)
        {
            var mesh = args.added[0];
            placedObject = Instantiate(placedPrefab, mesh.transform.position, Quaternion.identity);
            doneSpawn = true;
        }
    }
    private void PlaneChanged(ARPlanesChangedEventArgs args)
    {
        if(args.added != null && placedObject == null)
        {
            ARPlane arPlane = args.added[0];
             if (arPlane.size.x * arPlane.size.y > 0.1f){
            
            placedObject = Instantiate(placedPrefab, arPlane.transform.position, Quaternion.identity);
      
                DisablePlanes();
            

        
            doneSpawn = true;
             }

        }
    }

    

    private void Dismiss()
    { welcomePanel.SetActive(false);
      FindObjectOfType<SettingsMenu>().open = true;
    
    
     }


      public void Jump(){
        FindObjectOfType<DragonController>().Jump();
    }

       void DisablePlanes()
    {
        ARPlaneManager planeManager = GetComponent<ARPlaneManager>();
        planeManager.enabled = false;
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }


    }
   

}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TapToPlaceObject : MonoBehaviour
{
    private ARRaycastManager arOrigin;

    public GameObject placementIndicator;
    public GameObject objectToPlace;
    private Pose placementPose;
    private bool placementPoseIsValid = false;

    
    [SerializeField]
    private GameObject welcomePanel;


    private GameObject placedObject;

    [SerializeField]
    private Button dismissButton;


    public bool doneSpawn = false;
 

    void Awake() 
    {
        dismissButton.onClick.AddListener(Dismiss);
        doneSpawn = false;
          arOrigin = FindObjectOfType<ARRaycastManager>();

    }

  



    private void Dismiss()
    { welcomePanel.SetActive(false);
      FindObjectOfType<SettingsMenu>().open = true;
    
    
     }


    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (placedObject==null && placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){
            PlaceObject();
        }
        // else if (placedObject !=null){
          
        //      Vector3 directionToCamera = Camera.main.transform.position - placedObject.transform.position;
        //             directionToCamera.y = 0; // Keep the target upright
        //             Quaternion lookRotation = Quaternion.LookRotation(-directionToCamera);
        //             placedObject.transform.rotation = lookRotation;
        //     }
    
    }

    private void PlaceObject()
    {
        placedObject = Instantiate(objectToPlace,placementPose.position, placementPose.rotation);
            doneSpawn = true;
    }

    private void UpdatePlacementIndicator()
    {
      if (placedObject==null && placementPoseIsValid){
        placementIndicator.SetActive(true);
        placementIndicator.transform.SetPositionAndRotation(placementPose.position,placementPose.rotation);

      }
      else {
        placementIndicator.SetActive(false);
      }
    }

    private void UpdatePlacementPose(){
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f,0.5f,0.5f));
        var hits = new List<ARRaycastHit>();
        arOrigin.Raycast(screenCenter,hits,TrackableType.Planes);
    
        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid){
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;

            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
}

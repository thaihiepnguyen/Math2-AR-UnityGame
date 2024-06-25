using UnityEngine;
using Niantic.Lightship.AR.NavigationMesh;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;


public class NavMeshHowTo : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;


    // [SerializeField]
    // private LightshipNavMeshManager _navmeshManager;

    // [SerializeField]
    // private LightshipNavMeshAgent _agentPrefab;

    
    // private LightshipNavMeshAgent _agentInstance;



    [SerializeField]
    private GameObject _agentPrefab;

    
    private GameObject _agentInstance;

    private bool isPlaced = false;
    void Update()
    {
      if (isPlaced) return;
        HandleTouch();
    }

    

    // public void SetVisualization(bool isVisualizationOn)
    // {
    //     //turn off the rendering for the navmesh
    //     _navmeshManager.GetComponent<LightshipNavMeshRenderer>().enabled = isVisualizationOn;

    //     if (_agentInstance != null)
    //     {
    //         //turn off the path rendering on the active agent
    //         _agentInstance.GetComponent<LightshipNavMeshAgentPathRenderer>().enabled = isVisualizationOn;
    //     }
    // }



     private void HandleTouch()
    {
        //in the editor we want to use mouse clicks, on phones we want touches.
    #if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
    
        
    #else
        var touch = Input.GetTouch(0);

        //if there is no touch or touch selects UI element
        if (Input.touchCount <= 0)
            return;
        if (touch.phase == UnityEngine.TouchPhase.Began)
    #endif
        {
        #if UNITY_EDITOR
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        #else
            Ray ray = _camera.ScreenPointToRay(touch.position);
        #endif
            //project the touch point from screen space into 3d and pass that to your agent as a destination
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (_agentInstance == null )
                {
                    _agentInstance = Instantiate(_agentPrefab);
                    _agentInstance.transform.position = hit.point;
                    isPlaced = true;
                }
                // else
                // {
                //     _agentInstance.SetDestination(hit.point);
                // }
            }
        }
    }
}
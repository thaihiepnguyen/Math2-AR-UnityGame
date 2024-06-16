using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPortalCreator : MonoBehaviour
{
    [SerializeField] private GameObject portalPrefab;
    [SerializeField] private float delayBeforeARRaycast = 0.5f;

    ARRaycastManager arRaycastManager;
    ARPlaneManager arPlaneManager;

    List<ARRaycastHit> hits;
    GameObject portal;    


    private void Awake()
    {
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
        arPlaneManager = FindObjectOfType<ARPlaneManager>();
        hits = new List<ARRaycastHit>();
    }

    private void Start()
    {
        portal = Instantiate(portalPrefab);
        RestartHandler();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnPortalCreating += PortalCreationHandler;
        GameManager.Instance.OnRestart += RestartHandler;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnPortalCreating -= PortalCreationHandler;
        GameManager.Instance.OnRestart -= RestartHandler;
    }

    private void RestartHandler()
    {
        portal.SetActive(false);
        SetActiveARSession(true);
    }

    private void PortalCreationHandler()
    {
        SetStatusPortal(false);
        StartCoroutine(PortalCreationRoutine());
    }

    IEnumerator PortalCreationRoutine()
    {
        SetActiveARSession(true);
        bool isPortalCreating = true;

        var arCamera = GameManager.Instance.ARCamera;

        Vector2 middleScreenPoint = arCamera.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));

      
        yield return new WaitForSeconds(delayBeforeARRaycast);

        while (isPortalCreating)
        {
            if (arRaycastManager.Raycast(middleScreenPoint, hits, TrackableType.Planes))
            {
                var pose = hits[0].pose;
                portal.transform.SetPositionAndRotation(pose.position, pose.rotation);

                SetStatusPortal(true);

                if (InputARController.IsTapping())
                    isPortalCreating = false;
            }
            else
            {
                SetStatusPortal(false);
            }

#if UNITY_EDITOR
           
            if (InputARController.IsTapping()) 
            {
                SetStatusPortal(true);
                isPortalCreating = false;
            }
#endif
            yield return null;
        }
        
        SetActiveARSession(false);

        GameManager.Instance.PortalCreated(portal.transform);
    }

    private void SetStatusPortal(bool status)
    {
        portal.SetActive(status);
        GameManager.Instance.StatusPortal(status);
    }

    void SetActiveARSession(bool value)
    {
        arPlaneManager.enabled = value;
        arRaycastManager.enabled = value;        
    }

}

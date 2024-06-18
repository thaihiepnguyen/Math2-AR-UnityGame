using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnableManager : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private GameObject placementObject;

    private List<GameObject> placedObjects = new();

    private bool isPlaced = false;

    void Update()
    {
        if (isPlaced) return;
#if UNITY_EDITOR
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("UI Hit was recognized");
                return;
            }

            TouchToRay(mainCam.transform.position);
        }
#elif UNITY_IOS || UNITY_ANDROID
        
        {
            
            // PointerEventData pointerData = new PointerEventData(EventSystem.current);
            // pointerData.position = mainCam.transform.position;

            // List<RaycastResult> results = new List<RaycastResult>();

            // EventSystem.current.RaycastAll(pointerData, results);

            // if (results.Count > 0) {
            //     Debug.Log("We hit an UI Element");
            //     return;
            // }
            

            // Debug.Log("get here");
            TouchToRay(mainCam.transform.position + mainCam.transform.forward * 5);
        }
#endif
    }
    
    void TouchToRay(Vector3 touch)
    {
        touch.y -= 3;
        // Ray ray = mainCam.ScreenPointToRay(touch);
        // RaycastHit hit;
        // if (Physics.Raycast(ray ,out hit))
        // {
            // placedObjects.Add(
            //     Instantiate(placementObject, hit.point, Quaternion.FromToRotation(transform.up, hit.normal))
            //     );
            placedObjects.Add(Instantiate(placementObject,touch,Quaternion.identity));
                isPlaced = true;
        // }
    }
}
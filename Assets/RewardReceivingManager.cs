using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RewardReceivingManager : MonoBehaviour, IPointerClickHandler {


    GraphicRaycaster m_Raycaster;
    public void OnPointerClick(PointerEventData eventData)
    {
     
        m_Raycaster = transform.parent.GetComponent<GraphicRaycaster>();
        List<RaycastResult> results = new List<RaycastResult>();
        m_Raycaster.Raycast(eventData, results);
        foreach (RaycastResult result in results)
        {
            Debug.Log("Hit " + result.gameObject.name);
            if (result.gameObject.tag != "Outsider")
            {
                return;
            }
            else {
                // gameObject.SetActive(false);
                Destroy(gameObject);
            }
      
        }


    }
}

 

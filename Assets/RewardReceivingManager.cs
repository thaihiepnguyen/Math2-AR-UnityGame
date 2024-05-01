using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RewardReceivingManager : MonoBehaviour, IPointerClickHandler {


    GraphicRaycaster m_Raycaster;
    [SerializeField] private TextMeshProUGUI text;
    

    public void GetData(int price){
        text.text =  string.Format("x{0}",price.ToString());
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
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

 

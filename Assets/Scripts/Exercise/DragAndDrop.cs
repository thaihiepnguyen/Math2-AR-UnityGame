using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler,IDragHandler
{
    [HideInInspector]public Transform parentAfterDrag;
    Image image;
    public TextMeshProUGUI textMeshProUGUI;
    private  void Awake()
    {  
       image= GetComponent<Image>();
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("On begin drag");
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
        textMeshProUGUI.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("On drag");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("On end drag");
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
        textMeshProUGUI.raycastTarget = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("On mouse down");
    }

    // Start is called before the first frame update
    
}

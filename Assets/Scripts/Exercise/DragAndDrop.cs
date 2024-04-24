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
    [SerializeField]public Transform parentAfterDrag;
    [SerializeField] public Transform parentBeforeDrag;
    Image image;
    public TextMeshProUGUI textMeshProUGUI;
    private  void Awake()
    {  
        image= GetComponent<Image>();
        parentBeforeDrag = transform.parent;
        Debug.Log("before" + parentBeforeDrag.name);


    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
        textMeshProUGUI.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
        textMeshProUGUI.raycastTarget = true;
        parentBeforeDrag = parentAfterDrag;
        Debug.Log("after" + parentBeforeDrag.name);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("On mouse down");
    }

    // Start is called before the first frame update
    
}

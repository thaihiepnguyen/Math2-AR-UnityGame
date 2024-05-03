using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    
    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount== 0)
        {
            GameObject dropped = eventData.pointerDrag;
            DragAndDrop dragDropItem = dropped.GetComponent<DragAndDrop>();
            dragDropItem.parentAfterDrag = transform;
        }
        else if(transform.childCount == 1)
        {
            GameObject dropped = eventData.pointerDrag;
            DragAndDrop beginItem = dropped.GetComponent<DragAndDrop>();
            DragAndDrop endItem = transform.GetComponentInChildren<DragAndDrop>();
            Debug.Log("drop " + endItem.transform.parent.name);
            endItem.transform.SetParent(beginItem.parentBeforeDrag);
            endItem.parentAfterDrag = beginItem.parentBeforeDrag;
            endItem.parentBeforeDrag = beginItem.parentBeforeDrag;
            beginItem.parentAfterDrag = transform;
            
        }
      
    }

    // Start is called before the first frame update
    
}

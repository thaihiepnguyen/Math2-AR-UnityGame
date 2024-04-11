using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour,IDropHandler
{
    
    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount== 0)
        {
            GameObject dropped = eventData.pointerDrag;
            DragAndDrop draganddropItem = dropped.GetComponent<DragAndDrop>();
            draganddropItem.parentAfterDrag = transform;
        }
      
    }

    // Start is called before the first frame update
    
}

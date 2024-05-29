using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ButtonHoldAndRelease : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    // Start is called before the first frame update
    private bool isButtonHeld = false;

    // Define custom events
    public  event Action OnButtonDownEvent;
    public  event Action OnButtonUpEvent;
    public  event Action OnButtonHoldEvent;

    public void OnPointerDown(PointerEventData eventData)
    {
        
        isButtonHeld = true;
        //Debug.Log("Button Down");
        OnButtonDownEvent?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isButtonHeld = false;
        //Debug.Log("Button Up");
        OnButtonUpEvent?.Invoke();
    }

    void Update()
    {
        if (isButtonHeld)
        {
            //Debug.Log("Button Hold");
            OnButtonHoldEvent?.Invoke();
        }
    }
}

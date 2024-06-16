using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class InputARController
{
    static PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
    static List<RaycastResult> results = new List<RaycastResult>();

    public static bool IsTapping()
    {
#if UNITY_EDITOR      
        // Notar que se est� validando que al hacer click NO estemos sobre un elemento de la UI, como un bot�n.
        return (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)) && !EventSystem.current.IsPointerOverGameObject();
#else
        // Esto se supone que deber�a funcionar, pero no me reconoce que el touch est� sobre un elemento de UI: 
        //  siempre me retorna true cuando el dedo toca la pantalla.
        //if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))        
        //  return false;

        if (Input.touchCount > 0 && IsPointerOverUIObject())
          return false;

        return Input.touchCount > 0;
#endif
    }


    private static bool IsPointerOverUIObject()
    {
        // Notar que el touch tambi�n funciona con la posici�n del mouse
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y); 
        results.Clear();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

}

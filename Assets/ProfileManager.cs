using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
   

    [SerializeField]
    private GameObject canvas;
     void Awake(){
        LeanTween.reset();
    }

    // Start is called before the first frame update
 
    public void OnEnable()
    {
        float width = canvas.gameObject.GetComponent<RectTransform>().rect.width;
        Debug.Log(width);
         LeanTween.moveLocalX(gameObject, -width/2f, 0.5f).setEase(LeanTweenType.easeInOutCirc);
    }
}

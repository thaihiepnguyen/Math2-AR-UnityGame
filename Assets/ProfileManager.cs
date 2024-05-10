using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    [SerializeField] 
    private GameObject personal; 
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.moveX(personal,0, 3);
    }

}

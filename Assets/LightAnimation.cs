using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAnimation : MonoBehaviour
{
    // Start is called before the first frame update

    void Awake(){
        LeanTween.reset();
    }
    void Start()
    {
        LeanTween.rotateAround(gameObject, Vector3.forward, -360, 10f).setLoopClamp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

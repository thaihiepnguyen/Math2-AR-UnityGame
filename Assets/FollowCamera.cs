using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
   [SerializeField] GameObject thingToFollow;

  
    // void Start()
    // {
    //     thingToFollow = GameObject.FindGameObjectsWithTag("Player")[0];
    // }
    void LateUpdate()
    {
        if (thingToFollow == null){
            return;
        }
        
        transform.localPosition =  new Vector3(thingToFollow.transform.localPosition.x,10f,thingToFollow.transform.localPosition.z);
     
      
    }
}

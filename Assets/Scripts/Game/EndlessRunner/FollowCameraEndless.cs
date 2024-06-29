using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraEndless : MonoBehaviour
{
   

    // Update is called once per frame
    void Update()
    {
         
                Vector3 directionToCamera = Camera.main.transform.position - transform.position;
                    directionToCamera.y = 0; // Keep the target upright
                    Quaternion lookRotation = Quaternion.LookRotation(-directionToCamera);
                    transform.rotation = lookRotation;
    }
}

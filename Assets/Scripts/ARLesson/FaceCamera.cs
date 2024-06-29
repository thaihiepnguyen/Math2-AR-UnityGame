using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FaceCamera : MonoBehaviour
{
    // Start is called before the first frame update
   
   Transform cam;

   Vector3 targetAngle = Vector3.zero;

   void Start(){
    cam = Camera.main.transform;
   }

   void Update(){
    // transform.LookAt(cam);
    // targetAngle = transform.localEulerAngles;
    // targetAngle.x = 0;
    // targetAngle.z = 0;
    // transform.localEulerAngles = targetAngle;

           Vector3 directionToCamera = Camera.main.transform.position - transform.position;
                    directionToCamera.y = 0; // Keep the target upright
                    Quaternion lookRotation = Quaternion.LookRotation(-directionToCamera);
                    transform.rotation = lookRotation;
   }
}
